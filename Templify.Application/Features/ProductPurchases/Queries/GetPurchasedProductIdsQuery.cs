using MediatR;

namespace Templify.Application.Features.ProductPurchases.Queries;

public record GetPurchasedProductIdsQuery : IRequest<List<int>>
{
    public int AppUserId { get; set; }
}



