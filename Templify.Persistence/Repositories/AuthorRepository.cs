using Microsoft.EntityFrameworkCore;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;
using Templify.Domain.Entities;
using Templify.Persistence.Contexts;

namespace Templify.Persistence.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly IGenericRepository<Author> _repository;

    public AuthorRepository(IGenericRepository<Author> repository)
    {
        _repository = repository;
    }

    // Delegating standard IGenericRepository methods
    public IQueryable<Author> Entities => _repository.Entities;
    public Task<List<AuthorDto>> GetAllAsync() => _repository.GetAllAsync().ContinueWith(t => t.Result.Select(MapToDto).ToList());
    public async Task<AuthorDto?> GetByIdAsync(int id) 
    {
        var author = await _repository.Entities
            .Include(a => a.Products)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
        return author != null ? MapToDto(author) : null;
    }

    public async Task<Author?> GetEntityByIdAsync(int id)
    {
        return await _repository.Entities
            .Include(a => a.Products)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    public Task<Author> AddAsync(Author entity) => _repository.AddAsync(entity);
    public Task UpdateAsync(Author entity) => _repository.UpdateAsync(entity);
    public Task DeleteAsync(Author entity) => _repository.DeleteAsync(entity);
    public Task SaveChangesAsync() => _repository.SaveChangesAsync();

    // Specific Author methods
    public async Task<IEnumerable<Author>> GetActiveAuthorsAsync()
    {
        return await _repository.Entities
            .Include(a => a.Products)
            .Include(a => a.User)
            .Where(a => a.IsActive)
            .OrderByDescending(a => a.TotalProducts)
            .ToListAsync();
    }

    public async Task<Author?> GetByUserIdAsync(string userId)
    {
        return await _repository.Entities
            .Include(a => a.Products)
            .Include(a => a.User)
            .FirstOrDefaultAsync(a => a.UserId == userId && a.IsActive);
    }

    public async Task<IEnumerable<Author>> GetSeedAuthorsAsync()
    {
        return await _repository.Entities
            .Include(a => a.Products)
            .Where(a => a.IsSeedAuthor && a.IsActive)
            .OrderByDescending(a => a.TotalProducts)
            .ToListAsync();
    }

    public async Task<IEnumerable<Author>> GetUserAuthorsAsync()
    {
        return await _repository.Entities
            .Include(a => a.Products)
            .Include(a => a.User)
            .Where(a => !a.IsSeedAuthor && a.IsActive)
            .OrderByDescending(a => a.TotalProducts)
            .ToListAsync();
    }

    private static AuthorDto MapToDto(Author author)
    {
        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            Bio = author.Bio,
            AvatarUrl = author.AvatarUrl,
            Email = author.Email,
            Website = author.Website,
            SocialLinks = author.SocialLinks,
            TotalProducts = author.TotalProducts,
            TotalDownloads = author.TotalDownloads,
            Specialization = author.Specialization,
            UserId = author.UserId,
            IsSeedAuthor = author.IsSeedAuthor,
            IsActive = author.IsActive,
        };
    }
}
