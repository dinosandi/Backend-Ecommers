using System.Collections.Generic;
using System;

namespace Domain.Entities
{
    public class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        
        // Navigation property
        public ICollection<ProductCategory> ProductCategories { get; set; }
    }
}