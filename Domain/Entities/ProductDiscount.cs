using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ProductDiscount
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
        public decimal TotalPriceBeforeDiscount { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        
        // Navigation properties
        public ICollection<Product> Products { get; set; }
    }
}