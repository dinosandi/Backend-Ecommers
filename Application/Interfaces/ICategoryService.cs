using Application.DTOs;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICategoryService
    {
        Task<Category> CreateCategoryAsync(CategoryDto categoryDto);
        Task<Category> UpdateCategoryAsync(Guid id, CategoryDto categoryDto);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<bool> AddProductToCategoryAsync(Guid categoryId, Guid productId);
    }
}