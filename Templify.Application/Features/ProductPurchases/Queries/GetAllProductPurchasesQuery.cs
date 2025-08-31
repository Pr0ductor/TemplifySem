using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.ProductPurchases.Queries
{
    public record GetAllProductPurchasesQuery : IRequest<List<ProductPurchaseDto>>;
}
