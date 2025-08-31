using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Services;

namespace Templify.Application.Features.Authors.Queries
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, List<AuthorDto>>
    {
        private readonly IAuthorService _authorService;
        public GetAllAuthorsQueryHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        public async Task<List<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            var authors = await _authorService.GetActiveAuthorsAsync();
            return authors.ToList();
        }
    }
}

