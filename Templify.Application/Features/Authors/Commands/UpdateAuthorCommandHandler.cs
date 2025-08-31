using MediatR;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Templify.Application.Features.Authors.Commands
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, bool>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly ILogger<UpdateAuthorCommandHandler> _logger;

        public UpdateAuthorCommandHandler(
            IAuthorRepository authorRepository,
            IGenericRepository<AppUser> userRepository,
            ILogger<UpdateAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var existingAuthor = await _authorRepository.GetEntityByIdAsync(request.Id);
                if (existingAuthor == null)
                {
                    _logger.LogWarning("Author with ID {AuthorId} not found for update", request.Id);
                    return false;
                }

                // Обновляем свойства автора
                existingAuthor.Name = request.DisplayName;
                existingAuthor.Email = request.Email;
                existingAuthor.AvatarUrl = request.AvatarUrl;
                existingAuthor.Specialization = request.Specialization;
                existingAuthor.Bio = request.Description;

                // Если указан UserId, привязываем автора к пользователю
                if (!string.IsNullOrEmpty(request.UserId))
                {
                    if (int.TryParse(request.UserId, out int appUserId))
                    {
                        var appUser = await _userRepository.GetByIdAsync(appUserId);
                        if (appUser != null)
                        {
                            existingAuthor.UserId = appUser.IdentityId;
                            existingAuthor.User = appUser.Identity;
                        }
                        else
                        {
                            _logger.LogWarning("AppUser with ID {UserId} not found for author update", appUserId);
                        }
                    }
                }
                else
                {
                    // Если UserId пустой, отвязываем от пользователя
                    existingAuthor.UserId = null;
                    existingAuthor.User = null;
                }

                await _authorRepository.UpdateAsync(existingAuthor);
                await _authorRepository.SaveChangesAsync();

                _logger.LogInformation("Author updated successfully: {AuthorId}", existingAuthor.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating author with ID {AuthorId}", request.Id);
                return false;
            }
        }
    }
}
