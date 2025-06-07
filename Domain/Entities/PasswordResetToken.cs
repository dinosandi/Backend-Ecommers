using System;

namespace Domain.Entities
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; set; } = false;
        
        // Navigation property
        public User User { get; set; }
    }
}