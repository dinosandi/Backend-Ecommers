using System;

namespace Application.DTOs;

public class UpdateProductBundleDto
{
    public string Name { get; set; }
    public decimal DiscountPercentage { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<Guid> ProductIds { get; set; }
}
