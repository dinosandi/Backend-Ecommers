using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveFile(IFormFile file);
        bool DeleteFile(string fileUrl);
    }
}