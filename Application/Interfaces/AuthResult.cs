

namespace Application.Interfaces
{
    public class AuthResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public string RefreshToken { get; set; } // Untuk implementasi refresh token
        public DateTime? TokenExpiration { get; set; }
        public UserDto User { get; set; } // Data user yang ditambahkan
        public int Role { get; set; } // ID user yang ditambahkan
        
    }
}