using Application.DTOs;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Infrastructure.Persistence;

namespace Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        private readonly IFileStorageService _fileStorageService;

        public CategoryService(
            AppDbContext context,
            IFileStorageService fileStorageService)
        {
            _context = context;
            _fileStorageService = fileStorageService;
        }

        public async Task<Category> CreateCategoryAsync(CategoryDto categoryDto)
        {
            var category = new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                ImageUrl = categoryDto.ImageUrl
            };

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<Category> UpdateCategoryAsync(Guid id, CategoryDto categoryDto)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            category.Name = categoryDto.Name;
            category.Description = categoryDto.Description;
            
            if (!string.IsNullOrEmpty(categoryDto.ImageUrl))
            {
                // Delete old image if exists
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    _fileStorageService.DeleteFile(category.ImageUrl);
                }
                category.ImageUrl = categoryDto.ImageUrl;
            }

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return category;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.IsActive)
                .ToListAsync();
        }

        public async Task<bool> AddProductToCategoryAsync(Guid categoryId, Guid productId)
        {
            // Check if relationship already exists
            var exists = await _context.ProductCategories
                .AnyAsync(pc => pc.CategoryId == categoryId && pc.ProductId == productId);

            if (exists)
            {
                return true; // Already exists
            }

            var productCategory = new ProductCategory
            {
                CategoryId = categoryId,
                ProductId = productId
            };

            _context.ProductCategories.Add(productCategory);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}