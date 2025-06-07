namespace Application.DTOs.Bundle
{
    public class CreateBundleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<BundleItemDto> Items { get; set; }
    }

    public class BundleItemDto
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class BundleResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal TotalOriginalPrice { get; set; }
        public decimal TotalDiscountedPrice { get; set; }
        public decimal TotalSavings { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<BundleItemResponseDto> Items { get; set; }
    }

    public class BundleItemResponseDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}