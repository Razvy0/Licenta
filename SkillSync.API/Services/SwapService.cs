using SkillSync.Core.DTOs.Swaps;
using SkillSync.Core.Entities;
using SkillSync.Core.Enums;
using SkillSync.Core.Exceptions;
using SkillSync.Core.Interfaces.Repositories;
using SkillSync.Core.Interfaces.Services;

namespace SkillSync.API.Services;

public class SwapService : ISwapService
{
    private readonly ISwapRepository _swapRepo;
    private readonly ISkillRepository _skillRepo;

    public SwapService(ISwapRepository swapRepo, ISkillRepository skillRepo)
    {
        _swapRepo = swapRepo;
        _skillRepo = skillRepo;
    }

    public async Task<IEnumerable<SwapDto>> GetUserSwapsAsync(string userId)
    {
        var swaps = await _swapRepo.GetSwapsByUserIdAsync(userId);
        return swaps.Select(MapToDto);
    }

    public async Task<SwapDto> CreateSwapAsync(string requesterId, CreateSwapDto dto)
    {
        var offeredSkill = await _skillRepo.GetSkillWithDetailsAsync(dto.OfferedSkillId)
            ?? throw new NotFoundException("Skill", dto.OfferedSkillId);

        if (offeredSkill.UserId != requesterId)
            throw new BadRequestException("You can only offer your own skills.");

        var requestedSkill = await _skillRepo.GetSkillWithDetailsAsync(dto.RequestedSkillId)
            ?? throw new NotFoundException("Skill", dto.RequestedSkillId);

        if (requestedSkill.UserId == requesterId)
            throw new BadRequestException("You cannot request your own skill.");

        var swap = new SwapRequest
        {
            RequesterId = requesterId,
            ReceiverId = requestedSkill.UserId,
            OfferedSkillId = dto.OfferedSkillId,
            RequestedSkillId = dto.RequestedSkillId,
            Status = SwapStatus.Pending
        };

        await _swapRepo.AddAsync(swap);
        var created = await _swapRepo.GetSwapWithDetailsAsync(swap.Id);
        return MapToDto(created!);
    }

    public async Task<SwapDto> UpdateSwapStatusAsync(string userId, int swapId, UpdateSwapStatusDto dto)
    {
        var swap = await _swapRepo.GetSwapWithDetailsAsync(swapId)
            ?? throw new NotFoundException("SwapRequest", swapId);

        if (swap.ReceiverId != userId && swap.RequesterId != userId)
            throw new UnauthorizedException("You are not a participant in this swap.");

        if (dto.Status == SwapStatus.Accepted && swap.ReceiverId != userId)
            throw new BadRequestException("Only the receiver can accept a swap request.");

        if (dto.Status == SwapStatus.Accepted && swap.Status != SwapStatus.Pending)
            throw new BadRequestException("Only pending swaps can be accepted.");

        if (dto.Status == SwapStatus.Rejected && swap.Status != SwapStatus.Pending)
            throw new BadRequestException("Only pending swaps can be rejected.");

        if (dto.Status == SwapStatus.Cancelled && swap.Status != SwapStatus.Pending)
            throw new BadRequestException("Only pending swaps can be cancelled.");

        swap.Status = dto.Status;
        swap.UpdatedAt = DateTime.UtcNow;
        await _swapRepo.UpdateAsync(swap);

        return MapToDto(swap);
    }

    public async Task<SwapDto> ProposeTimeSlotAsync(string userId, int swapId, ProposeTimeSlotDto dto)
    {
        var swap = await _swapRepo.GetSwapWithDetailsAsync(swapId)
            ?? throw new NotFoundException("SwapRequest", swapId);

        if (swap.ReceiverId != userId)
            throw new BadRequestException("Only the receiver can propose a time slot.");

        if (swap.Status != SwapStatus.Accepted)
            throw new BadRequestException("Time slots can only be proposed for accepted swaps.");

        if (dto.TimeSlotEnd <= dto.TimeSlotStart)
            throw new BadRequestException("End time must be after start time.");

        swap.TimeSlotStart = dto.TimeSlotStart;
        swap.TimeSlotEnd = dto.TimeSlotEnd;
        swap.UpdatedAt = DateTime.UtcNow;
        await _swapRepo.UpdateAsync(swap);

        return MapToDto(swap);
    }

    public async Task<SwapDto> PickTimeAsync(string userId, int swapId, PickTimeDto dto)
    {
        var swap = await _swapRepo.GetSwapWithDetailsAsync(swapId)
            ?? throw new NotFoundException("SwapRequest", swapId);

        if (swap.RequesterId != userId)
            throw new BadRequestException("Only the requester can pick a meeting time.");

        if (swap.Status != SwapStatus.Accepted)
            throw new BadRequestException("Swap must be accepted to pick a time.");

        if (swap.TimeSlotStart == null || swap.TimeSlotEnd == null)
            throw new BadRequestException("The receiver must propose a time slot first.");

        if (dto.ScheduledDate < swap.TimeSlotStart || dto.ScheduledDate > swap.TimeSlotEnd)
            throw new BadRequestException("Selected time must be within the proposed time slot.");

        swap.ScheduledDate = dto.ScheduledDate;
        swap.Status = SwapStatus.Scheduled;
        swap.UpdatedAt = DateTime.UtcNow;
        await _swapRepo.UpdateAsync(swap);

        return MapToDto(swap);
    }

    public async Task<SwapDto> ValidateSwapAsync(string userId, int swapId)
    {
        var swap = await _swapRepo.GetSwapWithDetailsAsync(swapId)
            ?? throw new NotFoundException("SwapRequest", swapId);

        if (swap.RequesterId != userId && swap.ReceiverId != userId)
            throw new UnauthorizedException("You are not a participant in this swap.");

        if (swap.Status != SwapStatus.Scheduled
            && swap.Status != SwapStatus.ValidatedByRequester
            && swap.Status != SwapStatus.ValidatedByReceiver)
            throw new BadRequestException("Swap must be scheduled before it can be validated.");

        if (swap.RequesterId == userId)
            swap.RequesterValidated = true;
        else
            swap.ReceiverValidated = true;

        if (swap.RequesterValidated && swap.ReceiverValidated)
            swap.Status = SwapStatus.Completed;
        else if (swap.RequesterValidated)
            swap.Status = SwapStatus.ValidatedByRequester;
        else
            swap.Status = SwapStatus.ValidatedByReceiver;

        swap.UpdatedAt = DateTime.UtcNow;
        await _swapRepo.UpdateAsync(swap);

        return MapToDto(swap);
    }

    public async Task<SwapDto> InvalidateSwapAsync(string userId, int swapId)
    {
        var swap = await _swapRepo.GetSwapWithDetailsAsync(swapId)
            ?? throw new NotFoundException("SwapRequest", swapId);

        if (swap.RequesterId != userId && swap.ReceiverId != userId)
            throw new UnauthorizedException("You are not a participant in this swap.");

        if (swap.Status != SwapStatus.Scheduled
            && swap.Status != SwapStatus.ValidatedByRequester
            && swap.Status != SwapStatus.ValidatedByReceiver)
            throw new BadRequestException("Swap must be scheduled before it can be invalidated.");

        // Reset validation and go back to Accepted so they can reschedule
        swap.RequesterValidated = false;
        swap.ReceiverValidated = false;
        swap.ScheduledDate = null;
        swap.TimeSlotStart = null;
        swap.TimeSlotEnd = null;
        swap.Status = SwapStatus.Accepted;
        swap.UpdatedAt = DateTime.UtcNow;
        await _swapRepo.UpdateAsync(swap);

        return MapToDto(swap);
    }

    private static SwapDto MapToDto(SwapRequest s) => new()
    {
        Id = s.Id,
        RequesterId = s.RequesterId,
        RequesterName = s.Requester?.FullName ?? "",
        ReceiverId = s.ReceiverId,
        ReceiverName = s.Receiver?.FullName ?? "",
        OfferedSkillTitle = s.OfferedSkill?.Title ?? "",
        RequestedSkillTitle = s.RequestedSkill?.Title ?? "",
        Status = s.Status,
        ScheduledDate = s.ScheduledDate,
        TimeSlotStart = s.TimeSlotStart,
        TimeSlotEnd = s.TimeSlotEnd,
        RequesterValidated = s.RequesterValidated,
        ReceiverValidated = s.ReceiverValidated,
        CreatedAt = s.CreatedAt
    };
}
