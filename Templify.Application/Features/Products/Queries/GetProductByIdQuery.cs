using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Products.Queries
{
    public record GetProductByIdQuery : IRequest<ProductDto?>
    {
        public int Id { get; set; }
    }
}

