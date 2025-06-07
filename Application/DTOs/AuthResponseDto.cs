namespace Application.DTOs
{
    public class AuthResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public int ExpiresIn { get; set; } = 3600; // default 1 jam
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
