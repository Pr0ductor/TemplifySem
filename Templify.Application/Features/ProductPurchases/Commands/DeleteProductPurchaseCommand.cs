using MediatR;

namespace Templify.Application.Features.ProductPurchases.Commands
{
    public record DeleteProductPurchaseCommand : IRequest<bool>
    {
        public int Id { get; init; }
    }
}
