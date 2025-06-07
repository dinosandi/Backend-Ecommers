using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        
        // Navigation properties
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<ProductDiscount> ProductDiscounts { get; set; } = new List<ProductDiscount>();
        public ICollection<TransactionDetail> TransactionDetails { get; set; }
        public ICollection<Review> Reviews { get; set; }
    }
}