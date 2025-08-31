using Templify.Application.Common.DTOs;
using Templify.Domain.Entities;

namespace Templify.Application.Interfaces.Services;

public interface IAuthorService
{
    Task<AuthorDto> GetAuthorByIdAsync(int id);
    Task<Author?> GetAuthorEntityByIdAsync(int id);
    Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync();
    Task<IEnumerable<AuthorDto>> GetActiveAuthorsAsync();
    Task<AuthorDto?> GetAuthorByUserIdAsync(string userId);
    Task<AuthorDto> GetOrCreateAuthorForUserAsync(string userId);
    Task<AuthorDto> CreateAuthorForUserAsync(string userId);
    Task<AuthorDto> UpdateAuthorAsync(int id, AuthorDto authorDto);
    Task UpdateAuthorStatsAsync(int authorId);
    Task<bool> DeleteAuthorAsync(int id);
    Task<bool> IsUserAuthorAsync(string userId);
    Task SyncAuthorWithUserAsync(string userId);
    Task<bool> IsUserAuthorOfProductAsync(string userId, int productId);
}

