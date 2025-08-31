using MediatR;
using System.Collections.Generic;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Products.Queries
{
    public record GetAllProductsQuery : IRequest<List<ProductDto>>
    {
    }
}

