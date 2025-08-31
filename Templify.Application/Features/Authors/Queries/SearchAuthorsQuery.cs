using MediatR;
using System.Collections.Generic;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Authors.Queries
{
    public record SearchAuthorsQuery : IRequest<List<AuthorDto>>
    {
        public string? SearchTerm { get; set; }
    }
}

