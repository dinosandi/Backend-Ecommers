using BCrypt.Net;

namespace Infrastructure.Services
{
    public class PasswordHasher
    {
        // Method untuk hash password
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Method untuk memverifikasi password
        public bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
