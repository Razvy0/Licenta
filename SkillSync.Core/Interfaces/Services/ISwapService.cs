using SkillSync.Core.DTOs.Swaps;

namespace SkillSync.Core.Interfaces.Services;

public interface ISwapService
{
    Task<IEnumerable<SwapDto>> GetUserSwapsAsync(string userId);
    Task<SwapDto> CreateSwapAsync(string requesterId, CreateSwapDto dto);
    Task<SwapDto> UpdateSwapStatusAsync(string userId, int swapId, UpdateSwapStatusDto dto);
}
