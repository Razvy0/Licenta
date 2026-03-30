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
            ScheduledDate = dto.ScheduledDate,
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

        swap.Status = dto.Status;
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
        CreatedAt = s.CreatedAt
    };
}
