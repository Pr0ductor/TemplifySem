using MediatR;

namespace Templify.Application.Features.ProductPurchases.Commands;

public record PurchaseProductCommand : IRequest<bool>
{
    public int ProductId { get; set; }
    public int AppUserId { get; set; }
}



