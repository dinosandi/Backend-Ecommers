// UpdateProductDto.cs
using Microsoft.AspNetCore.Http;

namespace Application.DTOs
{
    public class UpdateProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int? Stock { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
        public bool? IsActive { get; set; }
    }
}