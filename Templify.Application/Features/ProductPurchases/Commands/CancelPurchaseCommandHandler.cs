using MediatR;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.ProductPurchases.Commands;

public class CancelPurchaseCommandHandler : IRequestHandler<CancelPurchaseCommand, bool>
{
    private readonly IProductPurchaseService _purchaseService;
    public CancelPurchaseCommandHandler(IProductPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }
    public async Task<bool> Handle(CancelPurchaseCommand request, CancellationToken cancellationToken)
    {
        return await _purchaseService.CancelPurchaseAsync(request.ProductId, request.AppUserId);
    }
}



