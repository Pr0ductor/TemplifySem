using MediatR;
using System.Collections.Generic;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Authors.Queries
{
    public record GetAllAuthorsQuery : IRequest<List<AuthorDto>>
    {
    }
}

