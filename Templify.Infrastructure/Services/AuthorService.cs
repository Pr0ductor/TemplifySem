using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Repositories;
using Templify.Application.Interfaces.Services;
using Templify.Domain.Entities;
using Templify.Domain.Enums;

namespace Templify.Infrastructure.Services;

public class AuthorService : IAuthorService
{
    private readonly IAuthorRepository _authorRepository;
    private readonly IApplicationUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public AuthorService(
        IAuthorRepository authorRepository,
        IApplicationUserRepository userRepository,
        UserManager<ApplicationUser> userManager,
        IProductRepository productRepository)
    {
        _authorRepository = authorRepository;
        _userRepository = userRepository;
        _userManager = userManager;
        _productRepository = productRepository;
    }

    public async Task<AuthorDto> GetAuthorByIdAsync(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {id} not found");
        }
        
        return author;
    }

    public async Task<Author?> GetAuthorEntityByIdAsync(int id)
    {
        return await _authorRepository.GetEntityByIdAsync(id);
    }

    public async Task<IEnumerable<AuthorDto>> GetAllAuthorsAsync()
    {
        var authors = await _authorRepository.GetAllAsync();
        return authors;
    }

    public async Task<IEnumerable<AuthorDto>> GetActiveAuthorsAsync()
    {
        var authors = await _authorRepository.GetActiveAuthorsAsync();
        return authors.Select(MapToDto);
    }

    public async Task<AuthorDto?> GetAuthorByUserIdAsync(string userId)
    {
        var author = await _authorRepository.GetByUserIdAsync(userId);
        return author != null ? MapToDto(author) : null;
    }

    public async Task<AuthorDto> GetOrCreateAuthorForUserAsync(string userId)
    {
        // Check if user already has an author profile
        var existingAuthor = await _authorRepository.GetByUserIdAsync(userId);
        if (existingAuthor != null)
        {
            return MapToDto(existingAuthor);
        }

        // Create new author profile for user
        return await CreateAuthorForUserAsync(userId);
    }

    public async Task<AuthorDto> CreateAuthorForUserAsync(string userId)
    {
        // Get user data
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            throw new ArgumentException($"User with ID {userId} not found");
        }

        // Create author profile
        var author = new Author
        {
            Name = user.Username ?? user.Email?.Split('@')[0] ?? "Unknown Author",
            Bio = user.Description ?? "No bio available",
            AvatarUrl = !string.IsNullOrEmpty(user.Avatar) ? user.Avatar : "/src/img/person1.jpg",
            Email = user.Email ?? "",
            Website = "",
            SocialLinks = "",
            Specialization = "Designer",
            UserId = userId,
            IsSeedAuthor = false,
            IsActive = true,
            TotalProducts = 0,
            TotalDownloads = 0,
        };

        await _authorRepository.AddAsync(author);
        
        // Add "Author" role to user if not already present
        if (!await _userManager.IsInRoleAsync(user, "Author"))
        {
            var result = await _userManager.AddToRoleAsync(user, "Author");
            if (!result.Succeeded)
            {
                throw new InvalidOperationException($"Failed to add Author role to user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }

        return MapToDto(author);
    }

    public async Task<AuthorDto> UpdateAuthorAsync(int id, AuthorDto authorDto)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null)
        {
            throw new ArgumentException($"Author with ID {id} not found");
        }

        // Get the actual author entity to update
        var authorEntity = await _authorRepository.GetByUserIdAsync(author.UserId ?? "");
        if (authorEntity == null)
        {
            throw new ArgumentException($"Author entity not found for DTO with ID {id}");
        }

        // Update author properties
        authorEntity.Name = authorDto.Name;
        authorEntity.Bio = authorDto.Bio;
        authorEntity.AvatarUrl = authorDto.AvatarUrl;
        authorEntity.Email = authorDto.Email;
        authorEntity.Website = authorDto.Website;
        authorEntity.SocialLinks = authorDto.SocialLinks;
        authorEntity.Specialization = authorDto.Specialization;

        await _authorRepository.UpdateAsync(authorEntity);
        return MapToDto(authorEntity);
    }

    public async Task UpdateAuthorStatsAsync(int authorId)
    {
        var author = await _authorRepository.GetByIdAsync(authorId);
        if (author == null)
        {
            return;
        }

        // Get the actual author entity to update
        var authorEntity = await _authorRepository.GetByUserIdAsync(author.UserId ?? "");
        if (authorEntity == null)
        {
            return;
        }

        // Get products count and downloads from database
        var products = await _authorRepository.Entities
            .Where(a => a.Id == authorId)
            .SelectMany(a => a.Products)
            .ToListAsync();

        // Calculate stats from products
        authorEntity.TotalProducts = products.Count;
        authorEntity.TotalDownloads = products.Sum(p => p.Downloads);

        await _authorRepository.UpdateAsync(authorEntity);
    }

    public async Task<bool> DeleteAuthorAsync(int id)
    {
        var author = await _authorRepository.GetByIdAsync(id);
        if (author == null)
        {
            return false;
        }

        // Don't allow deletion of seed authors
        if (author.IsSeedAuthor)
        {
            return false;
        }

        // Get the actual author entity to update
        var authorEntity = await _authorRepository.GetByUserIdAsync(author.UserId ?? "");
        if (authorEntity == null)
        {
            return false;
        }

        // Soft delete by setting IsActive to false
        authorEntity.IsActive = false;

        await _authorRepository.UpdateAsync(authorEntity);
        return true;
    }

    public async Task<bool> IsUserAuthorAsync(string userId)
    {
        var author = await _authorRepository.GetByUserIdAsync(userId);
        return author != null && author.IsActive;
    }

    public async Task SyncAuthorWithUserAsync(string userId)
    {
        // Get user data
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return;
        }

        // Get author profile for this user
        var author = await _authorRepository.GetByUserIdAsync(userId);
        if (author == null)
        {
            return; // User doesn't have an author profile yet
        }

        // Sync author data with user data
        author.Name = user.Username ?? user.Email?.Split('@')[0] ?? "Unknown Author";
        author.Bio = user.Description ?? "No bio available";
        author.AvatarUrl = !string.IsNullOrEmpty(user.Avatar) ? user.Avatar : "/src/img/person1.jpg";
        author.Email = user.Email ?? "";

        await _authorRepository.UpdateAsync(author);
    }

    public async Task<bool> IsUserAuthorOfProductAsync(string userId, int productId)
    {
        try
        {
            var author = await _authorRepository.GetByUserIdAsync(userId);
            if (author == null)
            {
                return false;
            }

            var product = await _productRepository.Entities.FirstOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                return false;
            }

            return product.AuthorId == author.Id;
        }
        catch
        {
            return false;
        }
    }

    private static string FixImagePath(string path)
    {
        if (string.IsNullOrEmpty(path))
            return "/src/img/person1.jpg";
        if (path.StartsWith("~/"))
            path = path.Substring(2);
        if (!path.StartsWith("/"))
            path = "/" + path;
        return path;
    }

    private static AuthorDto MapToDto(Author author)
    {
        return new AuthorDto
        {
            Id = author.Id,
            Name = author.Name,
            Bio = author.Bio,
            AvatarUrl = FixImagePath(author.AvatarUrl),
            Email = author.Email,
            Website = author.Website,
            SocialLinks = author.SocialLinks,
            TotalProducts = author.Products?.Count ?? 0,
            TotalDownloads = author.Products?.Sum(p => p.Downloads) ?? 0,
            Specialization = author.Specialization,
            UserId = author.UserId,
            IsSeedAuthor = author.IsSeedAuthor,
            IsActive = author.IsActive,
        };
    }
}

