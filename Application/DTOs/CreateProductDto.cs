using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        [Required]
        [Range(00.0, double.MaxValue)]
        public decimal Price { get; set; }
        
        public int Stock { get; set; }
        
        public IFormFile ImageFile { get; set; }
        public string? ImageUrl { get; set; }
    }
}