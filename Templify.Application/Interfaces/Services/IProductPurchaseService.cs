namespace Templify.Application.Interfaces.Services;

public interface IProductPurchaseService
{
    Task<bool> PurchaseProductAsync(int productId, int appUserId);
    Task<bool> CancelPurchaseAsync(int productId, int appUserId);
    Task<bool> HasPurchasedAsync(int productId, int appUserId);
    Task<List<int>> GetPurchasedProductIdsAsync(int appUserId);
    Task<List<int>> GetBuyerIdsAsync(int productId);
    Task<int> GetBuyerCountAsync(int productId);
}



