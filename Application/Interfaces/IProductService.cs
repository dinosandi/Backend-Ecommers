using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProductAsync(CreateProductDto productDto);
        Task<Product> GetProductByIdAsync(Guid id);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> UpdateProductAsync(Guid id, UpdateProductDto productDto);
        Task<bool> DeleteProductAsync(Guid id);
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId);
    }
}