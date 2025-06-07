using Microsoft.AspNetCore.Http;

namespace Application.Interfaces
{
    public interface IImageService
    {
        Task<string> UploadImageAsync(IFormFile file);
    }
}