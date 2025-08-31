using MediatR;

namespace Templify.Application.Features.ProductPurchases.Queries;

public record HasPurchasedQuery : IRequest<bool>
{
    public int ProductId { get; set; }
    public int AppUserId { get; set; }
}



