using MediatR;
using Templify.Application.Common.DTOs;

namespace Templify.Application.Features.Authors.Queries
{
    public record GetAuthorByIdQuery : IRequest<AuthorDto?>
    {
        public int Id { get; init; }
    }
}

