using System;

namespace Domain.Entities
{
    public class Review
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public Guid ProductId { get; set; }
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public User User { get; set; }
        public Product Product { get; set; }
    }
}