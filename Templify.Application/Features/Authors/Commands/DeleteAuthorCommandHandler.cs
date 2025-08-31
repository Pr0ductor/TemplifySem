using MediatR;
using Templify.Application.Interfaces.Repositories;
using Microsoft.Extensions.Logging;

namespace Templify.Application.Features.Authors.Commands
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, bool>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<DeleteAuthorCommandHandler> _logger;

        public DeleteAuthorCommandHandler(
            IAuthorRepository authorRepository,
            ILogger<DeleteAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingAuthor = await _authorRepository.GetEntityByIdAsync(request.Id);
                if (existingAuthor == null)
                {
                    _logger.LogWarning("Author with ID {AuthorId} not found for deletion", request.Id);
                    return false;
                }

                await _authorRepository.DeleteAsync(existingAuthor);
                await _authorRepository.SaveChangesAsync();

                _logger.LogInformation("Author deleted successfully: {AuthorId}", existingAuthor.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting author with ID {AuthorId}", request.Id);
                return false;
            }
        }
    }
}
