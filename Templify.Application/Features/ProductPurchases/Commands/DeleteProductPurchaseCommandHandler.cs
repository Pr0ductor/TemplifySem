using MediatR;
using Templify.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Templify.Application.Features.ProductPurchases.Commands
{
    public class DeleteProductPurchaseCommandHandler : IRequestHandler<DeleteProductPurchaseCommand, bool>
    {
        private readonly IProductPurchaseRepository _purchaseRepository;
        private readonly ILogger<DeleteProductPurchaseCommandHandler> _logger;

        public DeleteProductPurchaseCommandHandler(
            IProductPurchaseRepository purchaseRepository,
            ILogger<DeleteProductPurchaseCommandHandler> logger)
        {
            _purchaseRepository = purchaseRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteProductPurchaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingPurchase = await _purchaseRepository.GetEntityByIdAsync(request.Id);
                if (existingPurchase == null)
                {
                    _logger.LogWarning("ProductPurchase with ID {PurchaseId} not found for deletion", request.Id);
                    return false;
                }

                await _purchaseRepository.DeleteAsync(existingPurchase);
                await _purchaseRepository.SaveChangesAsync();

                _logger.LogInformation("ProductPurchase deleted successfully: {PurchaseId}", existingPurchase.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting ProductPurchase with ID {PurchaseId}", request.Id);
                return false;
            }
        }
    }
}
