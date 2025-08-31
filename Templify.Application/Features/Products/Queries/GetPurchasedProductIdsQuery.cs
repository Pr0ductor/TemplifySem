using MediatR;

namespace Templify.Application.Features.Products.Queries;

public record GetPurchasedProductIdsQuery : IRequest<List<int>>
{
    public int AppUserId { get; set; }
}

