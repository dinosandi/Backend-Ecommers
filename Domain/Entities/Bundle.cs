using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Bundle
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime UpdatedAt { get; set; }
        public decimal TotalPriceBeforeDiscount { get; set; }
        public decimal TotalPriceAfterDiscount { get; set; }
        public ICollection<BundleItem> BundleItems { get; set; }
    }
}