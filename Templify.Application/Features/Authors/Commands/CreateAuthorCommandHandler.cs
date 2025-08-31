using MediatR;
using Templify.Application.Interfaces.Repositories;
using Templify.Application.Interfaces.Services;
using Templify.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Templify.Application.Features.Authors.Commands
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, bool>
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IGenericRepository<AppUser> _userRepository;
        private readonly ILogger<CreateAuthorCommandHandler> _logger;

        public CreateAuthorCommandHandler(
            IAuthorRepository authorRepository,
            IGenericRepository<AppUser> userRepository,
            ILogger<CreateAuthorCommandHandler> logger)
        {
            _authorRepository = authorRepository;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task<bool> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var author = new Author
                {
                    Name = request.DisplayName,
                    Email = request.Email,
                    AvatarUrl = request.AvatarUrl,
                    Specialization = request.Specialization,
                    Bio = request.Description,
                    CreatedDate = DateTime.UtcNow
                };

                // Если указан UserId, привязываем автора к пользователю
                if (!string.IsNullOrEmpty(request.UserId))
                {
                    if (int.TryParse(request.UserId, out int appUserId))
                    {
                        var appUser = await _userRepository.GetByIdAsync(appUserId);
                        if (appUser != null)
                        {
                            author.UserId = appUser.IdentityId; // Используем IdentityId из AppUser
                            author.User = appUser.Identity; // Привязываем к ApplicationUser
                        }
                        else
                        {
                            _logger.LogWarning("AppUser with ID {UserId} not found for author creation", appUserId);
                        }
                    }
                }

                await _authorRepository.AddAsync(author);
                await _authorRepository.SaveChangesAsync();

                _logger.LogInformation("Author created successfully: {AuthorId}", author.Id);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating author");
                return false;
            }
        }
    }
}
