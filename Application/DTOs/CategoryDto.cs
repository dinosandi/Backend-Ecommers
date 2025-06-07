using Microsoft.AspNetCore.Http;
using System;

namespace Application.DTOs
{
    public class CategoryDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }
        public string ImageUrl { get; set; }
    }
}