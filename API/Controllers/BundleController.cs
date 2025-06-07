using Microsoft.AspNetCore.Mvc;
using Application.DTOs.Bundle;
using Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BundleController : ControllerBase
    {
        private readonly IBundleService _bundleService;
        private readonly ILogger<BundleController> _logger;

        public BundleController(IBundleService bundleService, ILogger<BundleController> logger)
        {
            _bundleService = bundleService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<BundleResponseDto>> CreateBundle([FromBody] CreateBundleDto dto)
        {
            try
            {
                // Additional validation for null fields
                if (string.IsNullOrEmpty(dto.Name))
                {
                    return BadRequest("Name is required");
                }

                if (dto.Items == null || !dto.Items.Any())
                {
                    return BadRequest("Items are required");
                }

                if (dto.Items.Count < 2)
                {
                    return BadRequest("Bundle must contain at least 2 products");
                }

                if (dto.DiscountPercentage <= 0 || dto.DiscountPercentage > 100)
                {
                    return BadRequest("Discount percentage must be between 0 and 100");
                }

                if (dto.StartDate >= dto.EndDate)
                {
                    return BadRequest("Start date must be before end date");
                }

                // Validate product IDs
                foreach (var item in dto.Items)
                {
                    if (item.ProductId == Guid.Empty)
                    {
                        return BadRequest("Invalid Product ID");
                    }
                    if (item.Quantity <= 0)
                    {
                        return BadRequest("Quantity must be greater than 0");
                    }
                }

                var result = await _bundleService.CreateBundleAsync(dto);
                return CreatedAtAction(nameof(GetBundleById), new { id = result.Id }, result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bundle: {Message}", ex.Message);
                return StatusCode(500, $"An error occurred while creating the bundle: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BundleResponseDto>> GetBundleById(Guid id)
        {
            try
            {
                var bundle = await _bundleService.GetBundleByIdAsync(id);
                if (bundle == null)
                {
                    return NotFound();
                }
                return Ok(bundle);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bundle: {Message}", ex.Message);
                return StatusCode(500, "An error occurred while retrieving the bundle");
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<BundleResponseDto>>> GetAllBundles()
        {
            try
            {
                var bundles = await _bundleService.GetAllBundlesAsync();
                return Ok(bundles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bundles: {Message}", ex.Message);
                return StatusCode(500, "An error occurred while retrieving the bundles");
            }
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBundle(Guid id)
        {
            try
            {
                await _bundleService.DeleteBundleAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting bundle: {Message}", ex.Message);
                return StatusCode(500, "An error occurred while deleting the bundle");
            }
        }
    }
}