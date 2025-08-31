using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Templify.Application.Interfaces.Services;
using Templify.Domain.Entities;
using Templify.Application.Interfaces.Repositories;

namespace Templify.Infrastructure.Services;

public class ProductPurchaseService : IProductPurchaseService
{
    private readonly IGenericRepository<ProductPurchase> _purchaseRepository;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<AppUser> _appUserRepository;
    private readonly IGenericRepository<Author> _authorRepository; // Added for author download count update
    private readonly ILogger<ProductPurchaseService> _logger;

    public ProductPurchaseService(
        IGenericRepository<ProductPurchase> purchaseRepository,
        IGenericRepository<Product> productRepository,
        IGenericRepository<AppUser> appUserRepository,
        IGenericRepository<Author> authorRepository, // Added for author download count update
        ILogger<ProductPurchaseService> logger)
    {
        _purchaseRepository = purchaseRepository;
        _productRepository = productRepository;
        _appUserRepository = appUserRepository;
        _authorRepository = authorRepository; // Added for author download count update
        _logger = logger;
    }

    public async Task<bool> PurchaseProductAsync(int productId, int appUserId)
    {
        try
        {
            Console.WriteLine($"ProductPurchaseService: Starting purchase for product {productId} by user {appUserId}");
            
            var exists = await _purchaseRepository.Entities.AnyAsync(p => p.ProductId == productId && p.AppUserId == appUserId);
            Console.WriteLine($"ProductPurchaseService: Product already purchased: {exists}");
            
            if (exists)
            {
                Console.WriteLine("ProductPurchaseService: Product already purchased, returning false");
                return false;
            }
            
            var product = await _productRepository.GetByIdAsync(productId);
            Console.WriteLine($"ProductPurchaseService: Product found: {product != null}");
            
            var appUser = await _appUserRepository.GetByIdAsync(appUserId);
            Console.WriteLine($"ProductPurchaseService: AppUser found: {appUser != null}");
            
            if (product == null || appUser == null)
            {
                Console.WriteLine("ProductPurchaseService: Product or AppUser not found");
                return false;
            }
            
            var purchase = new ProductPurchase
            {
                ProductId = productId,
                AppUserId = appUserId,
                PurchasedAt = DateTime.UtcNow
            };
            
            Console.WriteLine("ProductPurchaseService: Creating purchase record");
            await _purchaseRepository.AddAsync(purchase);
            
            // Увеличиваем счетчик скачиваний у автора
            try
            {
                var author = await _authorRepository.GetByIdAsync(product.AuthorId);
                if (author != null)
                {
                    author.TotalDownloads++;
                    await _authorRepository.UpdateAsync(author);
                    Console.WriteLine($"ProductPurchaseService: Increased download count for author {author.Id} to {author.TotalDownloads}");
                }
                else
                {
                    Console.WriteLine($"ProductPurchaseService: Author not found for product {productId}");
                }
            }
            catch (Exception authorEx)
            {
                Console.WriteLine($"ProductPurchaseService: Error updating author download count: {authorEx.Message}");
                _logger.LogWarning(authorEx, "Failed to update author download count for product {ProductId}", productId);
                // Не прерываем покупку, если не удалось обновить счетчик автора
            }
            
            Console.WriteLine("ProductPurchaseService: Purchase successful");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ProductPurchaseService: Exception occurred: {ex.Message}");
            Console.WriteLine($"ProductPurchaseService: StackTrace: {ex.StackTrace}");
            _logger.LogError(ex, "Error purchasing product {ProductId} by user {AppUserId}", productId, appUserId);
            return false;
        }
    }

    public async Task<bool> CancelPurchaseAsync(int productId, int appUserId)
    {
        try
        {
            var purchase = await _purchaseRepository.Entities.FirstOrDefaultAsync(p => p.ProductId == productId && p.AppUserId == appUserId);
            if (purchase == null)
                return false;
            
            // Уменьшаем счетчик скачиваний у автора
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product != null)
                {
                    var author = await _authorRepository.GetByIdAsync(product.AuthorId);
                    if (author != null && author.TotalDownloads > 0)
                    {
                        author.TotalDownloads--;
                        await _authorRepository.UpdateAsync(author);
                        Console.WriteLine($"ProductPurchaseService: Decreased download count for author {author.Id} to {author.TotalDownloads}");
                    }
                }
            }
            catch (Exception authorEx)
            {
                Console.WriteLine($"ProductPurchaseService: Error updating author download count on cancel: {authorEx.Message}");
                _logger.LogWarning(authorEx, "Failed to update author download count on cancel for product {ProductId}", productId);
                // Не прерываем отмену покупки, если не удалось обновить счетчик автора
            }
            
            await _purchaseRepository.DeleteAsync(purchase);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error canceling purchase of product {ProductId} by user {AppUserId}", productId, appUserId);
            return false;
        }
    }

    public async Task<bool> HasPurchasedAsync(int productId, int appUserId)
    {
        try
        {
            return await _purchaseRepository.Entities.AnyAsync(p => p.ProductId == productId && p.AppUserId == appUserId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking purchase status for product {ProductId} and user {AppUserId}", productId, appUserId);
            return false;
        }
    }

    public async Task<List<int>> GetPurchasedProductIdsAsync(int appUserId)
    {
        try
        {
            return await _purchaseRepository.Entities.Where(p => p.AppUserId == appUserId).Select(p => p.ProductId).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting purchased product IDs for user {AppUserId}", appUserId);
            return new List<int>();
        }
    }

    public async Task<List<int>> GetBuyerIdsAsync(int productId)
    {
        try
        {
            return await _purchaseRepository.Entities.Where(p => p.ProductId == productId).Select(p => p.AppUserId).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting buyer IDs for product {ProductId}", productId);
            return new List<int>();
        }
    }

    public async Task<int> GetBuyerCountAsync(int productId)
    {
        try
        {
            return await _purchaseRepository.Entities.CountAsync(p => p.ProductId == productId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting buyer count for product {ProductId}", productId);
            return 0;
        }
    }
}

