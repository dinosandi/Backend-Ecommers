using System;

namespace Domain.Entities
{
    public class TransactionDetail
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid TransactionId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal PriceAtPurchase { get; set; }
        public decimal Subtotal { get; set; }
        
        // Navigation properties
        public Transaction Transaction { get; set; }
        public Product Product { get; set; }
    }
}