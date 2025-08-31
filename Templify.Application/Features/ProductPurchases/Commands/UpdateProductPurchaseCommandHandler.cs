using MediatR;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;

namespace Templify.Application.Features.ProductPurchases.Commands
{
    public class UpdateProductPurchaseCommandHandler : IRequestHandler<UpdateProductPurchaseCommand, bool>
    {
        private readonly IProductPurchaseRepository _purchaseRepository;

        public UpdateProductPurchaseCommandHandler(IProductPurchaseRepository purchaseRepository)
        {
            _purchaseRepository = purchaseRepository;
        }

        public async Task<bool> Handle(UpdateProductPurchaseCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var purchase = await _purchaseRepository.GetEntityByIdAsync(request.Id);
                if (purchase == null)
                {
                    return false;
                }

                purchase.AppUserId = request.UserId;
                purchase.ProductId = request.ProductId;
                purchase.PurchasedAt = DateTime.UtcNow;

                await _purchaseRepository.UpdateAsync(purchase);
                await _purchaseRepository.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
