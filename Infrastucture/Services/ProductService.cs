using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public ProductService(
            AppDbContext context,
            IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<Product> CreateProductAsync(CreateProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Stock = productDto.Stock,
                ImageUrl = productDto.ImageUrl
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .Where(p => p.IsActive)
                .ToListAsync();
        }

        public async Task<Product> UpdateProductAsync(Guid id, UpdateProductDto productDto)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                throw new KeyNotFoundException("Product not found");
            }

            product.Name = productDto.Name ?? product.Name;
            product.Description = productDto.Description ?? product.Description;
            product.Price = productDto.Price != 0 ? productDto.Price : product.Price;
            product.Stock = productDto.Stock ?? product.Stock;
            product.IsActive = productDto.IsActive ?? product.IsActive;

            if (!string.IsNullOrEmpty(productDto.ImageUrl))
            {
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    _fileStorageService.DeleteFile(product.ImageUrl);
                }
                product.ImageUrl = productDto.ImageUrl;
            }

            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return false;
            }

            product.IsActive = false;
            _context.Products.Update(product);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(Guid categoryId)
        {
            return await _context.Products
                .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                .ToListAsync();
        }
    }
}