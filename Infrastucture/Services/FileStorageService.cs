using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Application.Interfaces;


namespace Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly string _uploadPath;

        public FileStorageService(string uploadPath = "wwwroot/uploads")
        {
            _uploadPath = uploadPath;
            if (!Directory.Exists(_uploadPath))
            {
                Directory.CreateDirectory(_uploadPath);
            }
        }

        public async Task<string> SaveFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty");
            }

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_uploadPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{fileName}";
        }

        public bool DeleteFile(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
            {
                return false;
            }

            var fileName = Path.GetFileName(fileUrl);
            var filePath = Path.Combine(_uploadPath, fileName);

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
                return true;
            }

            return false;
        }
    }
}