using System.Threading.Tasks;
using Application.DTOs;

namespace Application.Interfaces
{
    public interface IAuthService
{
    Task<AuthResult> RegisterAsync(RegisterDto dto);
    Task<AuthResult> LoginAsync(LoginDto dto);
    Task<(bool Success, string Message)> GeneratePasswordResetTokenAsync(string email);
    Task<(bool Success, string Message)> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
}
}