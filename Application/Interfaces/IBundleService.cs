using Application.DTOs.Bundle;

namespace Application.Interfaces
{
    public interface IBundleService
    {
        Task<BundleResponseDto> CreateBundleAsync(CreateBundleDto dto);
        Task<BundleResponseDto> GetBundleByIdAsync(Guid id);
        Task<List<BundleResponseDto>> GetAllBundlesAsync();
        Task DeleteBundleAsync(Guid id);
    }
}