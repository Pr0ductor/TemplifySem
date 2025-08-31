using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Services;
using System.Linq;

namespace Templify.Application.Features.Authors.Queries
{
    public class SearchAuthorsQueryHandler : IRequestHandler<SearchAuthorsQuery, List<AuthorDto>>
    {
        private readonly IAuthorService _authorService;
        public SearchAuthorsQueryHandler(IAuthorService authorService)
        {
            _authorService = authorService;
        }
        public async Task<List<AuthorDto>> Handle(SearchAuthorsQuery request, CancellationToken cancellationToken)
        {
            var allAuthors = await _authorService.GetActiveAuthorsAsync();
            
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
                return allAuthors.ToList();
            
            // Поиск по частичному совпадению имени (без учета регистра)
            return allAuthors
                .Where(a => a.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}

