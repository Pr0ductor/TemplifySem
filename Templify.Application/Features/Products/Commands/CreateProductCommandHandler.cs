using MediatR;
using Microsoft.AspNetCore.Identity;
using Templify.Application.Common.DTOs;
using Templify.Application.Interfaces.Services;
using Templify.Domain.Entities;
using Templify.Domain.Enums;
using Templify.Domain.Entities;

namespace Templify.Application.Features.Products.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
{
    private readonly IProductService _productService;
    private readonly IAuthorService _authorService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INotificationService _notificationService;

    public CreateProductCommandHandler(
        IProductService productService,
        IAuthorService authorService,
        UserManager<ApplicationUser> userManager,
        INotificationService notificationService)
    {
        _productService = productService;
        _authorService = authorService;
        _userManager = userManager;
        _notificationService = notificationService;
    }

    public async Task<int> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Временное логирование для отладки
        Console.WriteLine($"CreateProductCommand received: UserId={request.UserId}, Name={request.Name}, Category={request.Category}, Price={request.Price}, AuthorId={request.AuthorId}");
        
        // 1. Get author - для админки используем AuthorId напрямую
        AuthorDto authorDto;
        if (int.TryParse(request.UserId, out int authorId) && authorId > 0)
        {
            // Это админка - используем AuthorId напрямую
            authorDto = await _authorService.GetAuthorByIdAsync(authorId);
            if (authorDto == null)
            {
                throw new ArgumentException($"Author with ID {authorId} not found");
            }
        }
        else
        {
            // Это обычный пользователь - получаем или создаем автора
            authorDto = await _authorService.GetOrCreateAuthorForUserAsync(request.UserId);
        }
        
        // Получаем полную сущность Author
        var author = await _authorService.GetAuthorEntityByIdAsync(authorDto.Id);
        if (author == null)
        {
            throw new ArgumentException($"Author entity with ID {authorDto.Id} not found");
        }
        
        Console.WriteLine($"Author retrieved: Id={author.Id}, Name={author.Name}");
        
        // 2. Map category to CategoryType
        var categoryType = MapCategoryToCategoryType(request.Category);
        Console.WriteLine($"Category mapped: {request.Category} -> {categoryType}");
        
        // 3. Create the product
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Category = request.Category,
            CategoryType = categoryType,
            Tags = request.Tags,
            ImageUrl = string.IsNullOrEmpty(request.ImageUrl) ? string.Empty : request.ImageUrl,
            AuthorId = author.Id,
            Author = author.Email, // Заполняем поле Author email'ом автора
            Downloads = 0,
        };

        Console.WriteLine($"Product entity created: Name={product.Name}, AuthorId={product.AuthorId}, Author={product.Author}");

        var createdProduct = await _productService.CreateProductAsync(product);
        Console.WriteLine($"Product saved to database: Id={createdProduct.Id}");
        
        // 4. Update author stats
        await _authorService.UpdateAuthorStatsAsync(author.Id);
        Console.WriteLine($"Author stats updated for author {author.Id}");
        
        // 5. Sync author data with user data to ensure email and other fields are up to date
        // Только для реальных пользователей, не для админки
        if (!int.TryParse(request.UserId, out _))
        {
            await _authorService.SyncAuthorWithUserAsync(request.UserId);
            Console.WriteLine($"Author data synced with user {request.UserId}");
            
            // 6. Assign Author role to user if not already assigned
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user != null && !await _userManager.IsInRoleAsync(user, "Author"))
            {
                await _userManager.AddToRoleAsync(user, "Author");
                Console.WriteLine($"Author role assigned to user {request.UserId}");
            }
            else
            {
                Console.WriteLine($"User {request.UserId} already has Author role or user not found");
            }
        }
        else
        {
            Console.WriteLine($"Skipping user sync for admin operation with UserId: {request.UserId}");
        }
        
        // 7. Notify subscribers
        await _notificationService.NotifyAuthorSubscribersProductPublishedAsync(author.Id, createdProduct.Id, createdProduct.Name);

        return createdProduct.Id;
    }

    private static CategoryType MapCategoryToCategoryType(string category)
    {
        return category.ToLower() switch
        {
            "business" => CategoryType.Business,
            "3d web" => CategoryType.ThreeDWeb,
            "saas platforms" => CategoryType.SaasPlatforms,
            "agency" => CategoryType.Agency,
            "portfolio design" => CategoryType.PortfolioDesign,
            "ecommerce" => CategoryType.Ecommerce,
            "education" => CategoryType.Education,
            "health" => CategoryType.Health,
            "marketing" => CategoryType.Marketing,
            "restaurant & food" => CategoryType.RestaurantAndFood,
            "gaming & entertainment" => CategoryType.GamingAndEntertainment,
            "real estate" => CategoryType.RealEstate,
            _ => CategoryType.Business // Default fallback
        };
    }
}
