using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly JwtService _jwt;
        private readonly PasswordHasher _hasher;

        public AuthService(AppDbContext context, JwtService jwt, PasswordHasher hasher)
        {
            _context = context;
            _jwt = jwt;
            _hasher = hasher;
        }

        // Register a new user
        public async Task<AuthResult> RegisterAsync(RegisterDto dto)
        {
            if (_context.Users.Any(u => u.Email == dto.Email))
                return new AuthResult
                {
                    Success = false,
                    Message = "Email already exists"
                };

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = _hasher.HashPassword(dto.Password),
                Role = dto.Role // Ensure Role is set correctly (Role is an enum, but dto.Role should match the enum type)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwt.GenerateToken(user);

            return new AuthResult
            {
                Success = true,
                Message = "Registered successfully",
                Token = token,
                TokenExpiration = DateTime.UtcNow.AddHours(1), // Set token expiration (1 hour here)
                User = new UserDto
                {
                    Id = user.Id, // Use User's Id
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role.ToString() // Convert Role enum to string here
                },
            };
        }

        // Login existing user
        public async Task<AuthResult> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !_hasher.VerifyPassword(user.PasswordHash, dto.Password))
            {
                return new AuthResult
                {
                    Success = false,
                    Message = "Invalid credentials"
                };
            }

            var token = _jwt.GenerateToken(user);

            return new AuthResult
            {
                Success = true,
                Message = "Login successful",
                Token = token,
                TokenExpiration = DateTime.UtcNow.AddHours(1), // Set token expiration (1 hour here)
                User = new UserDto
                {
                    Id = user.Id, // Use User's Id
                    Email = user.Email,
                    FullName = user.FullName,
                    Role = user.Role.ToString() // Convert Role enum to string here
                },
            };
        }

        // Generate password reset token
        public async Task<(bool Success, string Message)> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                return (false, "Email not found");
            }

            var token = Guid.NewGuid().ToString();
            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddHours(2)
            };

            _context.PasswordResetTokens.Add(resetToken);
            await _context.SaveChangesAsync();

            return (true, "Reset password token generated");
        }

        // Reset password using reset token
        public async Task<(bool Success, string Message)> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == resetPasswordDto.Email);
            if (user == null)
            {
                return (false, "Invalid request");
            }

            var resetToken = await _context.PasswordResetTokens
                .FirstOrDefaultAsync(t =>
                    t.UserId == user.Id &&
                    t.Token == resetPasswordDto.Token &&
                    t.ExpiresAt >= DateTime.UtcNow &&
                    !t.IsUsed);

            if (resetToken == null)
            {
                return (false, "Invalid or expired token");
            }

            // Update password
            CreatePasswordHash(resetPasswordDto.NewPassword, out byte[] passwordHash);
            user.PasswordHash = Convert.ToBase64String(passwordHash);

            // Mark token as used
            resetToken.IsUsed = true;

            await _context.SaveChangesAsync();

            return (true, "Password has been reset successfully");
        }

        // Helper method to create password hash
        private void CreatePasswordHash(string password, out byte[] passwordHash)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
// Compare this snippet from Infrastructure/Services/JwtService.cs: