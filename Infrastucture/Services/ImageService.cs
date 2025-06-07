using Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> UploadImageAsync(IFormFile image)
        {
            // Pastikan folder ada
            string folderPath = Path.Combine(_env.WebRootPath, "uploads", "bundles");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // Generate unique filename
            string fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";
            string filePath = Path.Combine(folderPath, fileName);

            // Simpan file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            // Return URL relatif
            return $"/uploads/bundles/{fileName}";
        }
    }
}
