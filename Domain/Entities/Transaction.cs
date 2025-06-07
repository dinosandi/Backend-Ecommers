using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } // Pending, Paid, Shipped, Delivered, Cancelled
        public string PaymentMethod { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        
        // Navigation properties
        public User User { get; set; }
        public ICollection<TransactionDetail> TransactionDetails { get; set; }
    }
}