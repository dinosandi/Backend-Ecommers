using System;

namespace Domain.Entities
{
    public class ProductCategory
    {
        public Guid ProductId { get; set; }
        public Guid CategoryId { get; set; }
        
        // Navigation properties
        public Product Product { get; set; }
        public Category Category { get; set; }
    }
}