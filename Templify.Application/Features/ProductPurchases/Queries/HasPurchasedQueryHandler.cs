using MediatR;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.ProductPurchases.Queries;

public class HasPurchasedQueryHandler : IRequestHandler<HasPurchasedQuery, bool>
{
    private readonly IProductPurchaseService _purchaseService;
    public HasPurchasedQueryHandler(IProductPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }
    public async Task<bool> Handle(HasPurchasedQuery request, CancellationToken cancellationToken)
    {
        return await _purchaseService.HasPurchasedAsync(request.ProductId, request.AppUserId);
    }
}



