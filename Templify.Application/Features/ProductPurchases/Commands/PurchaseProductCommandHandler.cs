using MediatR;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.ProductPurchases.Commands;

public class PurchaseProductCommandHandler : IRequestHandler<PurchaseProductCommand, bool>
{
    private readonly IProductPurchaseService _purchaseService;
    public PurchaseProductCommandHandler(IProductPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }
    public async Task<bool> Handle(PurchaseProductCommand request, CancellationToken cancellationToken)
    {
        return await _purchaseService.PurchaseProductAsync(request.ProductId, request.AppUserId);
    }
}



