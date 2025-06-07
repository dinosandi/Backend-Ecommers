namespace Domain.Entities
{
    public class BundleItem
    {
        public Guid Id { get; set; }
        public Guid BundleId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        
        public Bundle Bundle { get; set; }
        public Product Product { get; set; }
    }
}