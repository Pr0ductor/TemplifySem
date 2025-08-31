using Templify.Application.Common.DTOs;
using Templify.Domain.Entities;

namespace Templify.Application.Interfaces.Repositories;

public interface IAuthorRepository
{
    Task<List<AuthorDto>> GetAllAsync();
    Task<AuthorDto?> GetByIdAsync(int id);
    Task<Author?> GetEntityByIdAsync(int id);
    Task<Author> AddAsync(Author entity);
    Task UpdateAsync(Author entity);
    Task DeleteAsync(Author entity);
    Task SaveChangesAsync();
    IQueryable<Author> Entities { get; }
    Task<IEnumerable<Author>> GetActiveAuthorsAsync();
    Task<Author?> GetByUserIdAsync(string userId);
    Task<IEnumerable<Author>> GetSeedAuthorsAsync();
    Task<IEnumerable<Author>> GetUserAuthorsAsync();
}

