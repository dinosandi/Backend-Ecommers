using Application.DTOs.Bundle;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{
    public class BundleService : IBundleService
    {
        private readonly AppDbContext _context;

        public BundleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BundleResponseDto> CreateBundleAsync(CreateBundleDto dto)
        {
            // Validate bundle items
            if (dto.Items == null || dto.Items.Count < 2)
            {
                throw new ArgumentException("Bundle must contain at least 2 products");
            }

            // Get all product IDs from items
            var productIds = dto.Items.Select(i => i.ProductId).ToList();

            // Get products and validate they exist
            var products = await _context.Products
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            if (products.Count != productIds.Count)
            {
                throw new ArgumentException("One or more products not found");
            }

            // Calculate total prices and create bundle items
            decimal totalOriginalPrice = 0;
            var bundleItems = new List<BundleItem>();

            foreach (var item in dto.Items)
            {
                var product = products[item.ProductId];
                var itemTotal = product.Price * item.Quantity;
                totalOriginalPrice += itemTotal;

                bundleItems.Add(new BundleItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            // Calculate discount
            decimal discountAmount = totalOriginalPrice * (dto.DiscountPercentage / 100m);
            decimal totalAfterDiscount = totalOriginalPrice - discountAmount;

            // Create bundle
            var bundle = new Bundle
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                DiscountPercentage = dto.DiscountPercentage,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                UpdatedAt = DateTime.UtcNow,
                TotalPriceBeforeDiscount = totalOriginalPrice,
                TotalPriceAfterDiscount = totalAfterDiscount,
                BundleItems = bundleItems
            };

            await _context.Bundles.AddAsync(bundle);
            await _context.SaveChangesAsync();

            // Create response
            return new BundleResponseDto
            {
                Id = bundle.Id,
                Name = bundle.Name,
                Description = bundle.Description,
                DiscountPercentage = bundle.DiscountPercentage,
                TotalOriginalPrice = totalOriginalPrice,
                TotalDiscountedPrice = totalAfterDiscount,
                TotalSavings = discountAmount,
                StartDate = bundle.StartDate,
                EndDate = bundle.EndDate,
                Items = bundleItems.Select(bi => new BundleItemResponseDto
                {
                    ProductId = bi.ProductId,
                    ProductName = products[bi.ProductId].Name,
                    Quantity = bi.Quantity,
                    UnitPrice = products[bi.ProductId].Price,
                    TotalPrice = products[bi.ProductId].Price * bi.Quantity
                }).ToList()
            };
        }

        public async Task<BundleResponseDto> GetBundleByIdAsync(Guid id)
        {
            var bundle = await _context.Bundles
                .Include(b => b.BundleItems)
                    .ThenInclude(bi => bi.Product)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (bundle == null)
                throw new KeyNotFoundException("Bundle not found");

            return CreateBundleResponse(bundle);
        }

        public async Task<List<BundleResponseDto>> GetAllBundlesAsync()
        {
            var bundles = await _context.Bundles
                .Include(b => b.BundleItems)
                    .ThenInclude(bi => bi.Product)
                .ToListAsync();

            return bundles.Select(CreateBundleResponse).ToList();
        }

        public async Task<List<BundleResponseDto>> GetActiveBundlesAsync()
        {
            var currentDate = DateTime.UtcNow;
            var bundles = await _context.Bundles
                .Include(b => b.BundleItems)
                    .ThenInclude(bi => bi.Product)
                .Where(b => b.StartDate <= currentDate && b.EndDate >= currentDate)
                .ToListAsync();

            return bundles.Select(CreateBundleResponse).ToList();
        }

        private static BundleResponseDto CreateBundleResponse(Bundle bundle)
        {
            return new BundleResponseDto
            {
                Id = bundle.Id,
                Name = bundle.Name,
                Description = bundle.Description,
                DiscountPercentage = bundle.DiscountPercentage,
                TotalOriginalPrice = bundle.TotalPriceBeforeDiscount,
                TotalDiscountedPrice = bundle.TotalPriceAfterDiscount,
                TotalSavings = bundle.TotalPriceBeforeDiscount - bundle.TotalPriceAfterDiscount,
                StartDate = bundle.StartDate,
                EndDate = bundle.EndDate,
                Items = bundle.BundleItems.Select(bi => new BundleItemResponseDto
                {
                    ProductId = bi.ProductId,
                    ProductName = bi.Product.Name,
                    Quantity = bi.Quantity,
                    UnitPrice = bi.Product.Price,
                    TotalPrice = bi.Product.Price * bi.Quantity
                }).ToList()
            };
        }
        public async Task DeleteBundleAsync(Guid id)
        {
            var bundle = await _context.Bundles.FindAsync(id);
            if (bundle == null)
            {
                throw new KeyNotFoundException("Bundle not found");
            }

            _context.Bundles.Remove(bundle);
            await _context.SaveChangesAsync();
        }
    }
}