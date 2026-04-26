using SkillSync.Core.DTOs.Swaps;

namespace SkillSync.Core.Interfaces.Services;

public interface ISwapService
{
    Task<IEnumerable<SwapDto>> GetUserSwapsAsync(string userId);
    Task<SwapDto> CreateSwapAsync(string requesterId, CreateSwapDto dto);
    Task<SwapDto> UpdateSwapStatusAsync(string userId, int swapId, UpdateSwapStatusDto dto);
    Task<SwapDto> ProposeTimeSlotAsync(string userId, int swapId, ProposeTimeSlotDto dto);
    Task<SwapDto> PickTimeAsync(string userId, int swapId, PickTimeDto dto);
    Task<SwapDto> ValidateSwapAsync(string userId, int swapId);
    Task<SwapDto> InvalidateSwapAsync(string userId, int swapId);
}
