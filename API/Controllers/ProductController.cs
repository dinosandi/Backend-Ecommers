using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ecommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IFileStorageService _fileStorageService;

        public ProductsController(
            IProductService productService,
            IFileStorageService fileStorageService)
        {
            _productService = productService;
            _fileStorageService = fileStorageService;
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductDto productDto)
        {
            try
            {
                if (productDto.ImageFile != null)
                {
                    productDto.ImageUrl = await _fileStorageService.SaveFile(productDto.ImageFile);
                }

                var createdProduct = await _productService.CreateProductAsync(productDto);
                return CreatedAtAction(
                    nameof(GetProductById),
                    new { id = createdProduct.Id },
                    new
                    {
                        Id = createdProduct.Id,
                        Name = createdProduct.Name,
                        ImageUrl = createdProduct.ImageUrl,
                        Message = "Product created successfully"
                    });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProductById(Guid id)
        {
            try
            {
                var product = await _productService.GetProductByIdAsync(id);
                if (product == null)
                {
                    return NotFound(new { Message = "Product not found" });
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetAllProductsAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] UpdateProductDto productDto)
        {
            try
            {
                if (productDto.ImageFile != null)
                {
                    productDto.ImageUrl = await _fileStorageService.SaveFile(productDto.ImageFile);
                }

                var updatedProduct = await _productService.UpdateProductAsync(id, productDto);
                return Ok(new
                {
                    Id = updatedProduct.Id,
                    Name = updatedProduct.Name,
                    ImageUrl = updatedProduct.ImageUrl,
                    Message = "Product updated successfully"
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return Ok(new { Message = "Product deleted successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

    }
}