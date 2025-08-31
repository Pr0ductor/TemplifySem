using MediatR;
using System.Collections.Generic;
using Templify.Application.Common.DTOs;
using Templify.Domain.Enums;

namespace Templify.Application.Features.Products.Queries
{
    public record SearchProductsQuery : IRequest<List<ProductDto>>
    {
        public string? SearchTerm { get; set; }
        public CategoryType? CategoryType { get; set; }
    }
}

