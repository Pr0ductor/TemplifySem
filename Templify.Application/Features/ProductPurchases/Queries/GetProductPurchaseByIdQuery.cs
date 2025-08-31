using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.ProductPurchases.Queries
{
    public record GetProductPurchaseByIdQuery : IRequest<ProductPurchaseDto?>
    {
        public int Id { get; init; }
    }
}
