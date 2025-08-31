using MediatR;
using Templify.Application.Interfaces.Services;
using Templify.Application.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Templify.Application.Features.Products.Commands;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, bool>
{
    private readonly IProductService _productService;
    private readonly IAuthorService _authorService;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(
        IProductService productService, 
        IAuthorService authorService,
        ILogger<UpdateProductCommandHandler> logger)
    {
        _productService = productService;
        _authorService = authorService;
        _logger = logger;
    }

    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("UpdateProductCommand received: ProductId={ProductId}, UserId={UserId}", 
                request.Id, request.UserId);

            // Get the product to update
            var existingProduct = await _productService.GetProductByIdAsync(request.Id);
            if (existingProduct == null)
            {
                _logger.LogWarning("Product not found: {ProductId}", request.Id);
                return false;
            }

            _logger.LogInformation("Existing product found: {ProductName}", existingProduct.Name);

            // Verify that the user is the author of this product (skip for admin)
            if (!request.IsAdmin)
            {
                var isAuthor = await _authorService.IsUserAuthorOfProductAsync(request.UserId, request.Id);
                if (!isAuthor)
                {
                    _logger.LogWarning("User {UserId} is not author of product {ProductId}", request.UserId, request.Id);
                    return false;
                }
            }
            else
            {
                _logger.LogInformation("Admin update - skipping author verification for product {ProductId}", request.Id);
            }

            _logger.LogInformation("User {UserId} is confirmed as author of product {ProductId}", request.UserId, request.Id);

            // Get the actual product entity from repository to update
            var productToUpdate = await _productService.GetProductEntityByIdAsync(request.Id);
            if (productToUpdate == null)
            {
                _logger.LogWarning("Product entity not found for update: {ProductId}", request.Id);
                return false;
            }

            // Update only the fields that should be updated
            productToUpdate.Name = request.Name;
            productToUpdate.Description = request.Description;
            productToUpdate.Price = request.Price;
            productToUpdate.Category = request.Category;
            productToUpdate.CategoryType = MapCategoryToCategoryType(request.Category);
            productToUpdate.Tags = request.Tags;
            productToUpdate.ImageUrl = request.ImageUrl;
            // Preserve AuthorId, Downloads, and other fields

            _logger.LogInformation("Updated product entity: {ProductName}, Category: {Category}", 
                productToUpdate.Name, productToUpdate.Category);

            var result = await _productService.UpdateProductAsync(productToUpdate);
            _logger.LogInformation("Product update result: {Result}", result);
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating product {ProductId}", request.Id);
            return false;
        }
    }

    private static Domain.Enums.CategoryType MapCategoryToCategoryType(string category)
    {
        return category.ToLower() switch
        {
            "business" => Domain.Enums.CategoryType.Business,
            "3d web" => Domain.Enums.CategoryType.ThreeDWeb,
            "saas platforms" => Domain.Enums.CategoryType.SaasPlatforms,
            "agency" => Domain.Enums.CategoryType.Agency,
            "portfolio design" => Domain.Enums.CategoryType.PortfolioDesign,
            "ecommerce" => Domain.Enums.CategoryType.Ecommerce,
            "education" => Domain.Enums.CategoryType.Education,
            "health" => Domain.Enums.CategoryType.Health,
            "marketing" => Domain.Enums.CategoryType.Marketing,
            "restaurant & food" => Domain.Enums.CategoryType.RestaurantAndFood,
            "gaming & entertainment" => Domain.Enums.CategoryType.GamingAndEntertainment,
            "real estate" => Domain.Enums.CategoryType.RealEstate,
            _ => Domain.Enums.CategoryType.Business
        };
    }
}
