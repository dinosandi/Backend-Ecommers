using System;
using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly IFileStorageService _fileStorageService;

        public CategoriesController(
            ICategoryService categoryService,
            IFileStorageService fileStorageService)
        {
            _categoryService = categoryService;
            _fileStorageService = fileStorageService;
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateCategory([FromForm] CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto.ImageFile != null)
                {
                    categoryDto.ImageUrl = await _fileStorageService.SaveFile(categoryDto.ImageFile);
                }

                var result = await _categoryService.CreateCategoryAsync(categoryDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(Guid id, [FromForm] CategoryDto categoryDto)
        {
            if (categoryDto.ImageFile != null)
            {
                categoryDto.ImageUrl = await _fileStorageService.SaveFile(categoryDto.ImageFile);
            }

            var result = await _categoryService.UpdateCategoryAsync(id, categoryDto);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return Ok(categories);
        }


        [HttpPost("{categoryId}/products/{productId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProductToCategory(Guid categoryId, Guid productId)
        {
            var result = await _categoryService.AddProductToCategoryAsync(categoryId, productId);
            return Ok(result);
        }
    }
}
