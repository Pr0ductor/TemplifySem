using MediatR;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.ProductPurchases.Queries;

public class GetPurchasedProductIdsQueryHandler : IRequestHandler<GetPurchasedProductIdsQuery, List<int>>
{
    private readonly IProductPurchaseService _purchaseService;
    public GetPurchasedProductIdsQueryHandler(IProductPurchaseService purchaseService)
    {
        _purchaseService = purchaseService;
    }
    public async Task<List<int>> Handle(GetPurchasedProductIdsQuery request, CancellationToken cancellationToken)
    {
        return await _purchaseService.GetPurchasedProductIdsAsync(request.AppUserId);
    }
}



