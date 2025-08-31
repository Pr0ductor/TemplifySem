using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;

namespace Templify.Application.Features.Authors.Queries
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, AuthorDto?>
    {
        private readonly IAuthorRepository _authorRepository;

        public GetAuthorByIdQueryHandler(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<AuthorDto?> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            return await _authorRepository.GetByIdAsync(request.Id);
        }
    }
}

