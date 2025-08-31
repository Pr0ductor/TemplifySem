using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Templify.Application.Features.Products.Commands;

public record CreateProductCommand : IRequest<int>
{
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 and 100 characters")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "Product description is required")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "Product description must be between 10 and 2000 characters")]
    public string Description { get; init; } = string.Empty;

    [Required(ErrorMessage = "Price is required")]
    [Range(0.01, 9999.99, ErrorMessage = "Price must be between $0.01 and $9999.99")]
    public decimal Price { get; init; }

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; init; } = string.Empty;

    [StringLength(200, ErrorMessage = "Tags cannot exceed 200 characters")]
    public string Tags { get; init; } = string.Empty;

    public string ImageUrl { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "Author is required")]
    public int AuthorId { get; init; }
    
    public string UserId { get; init; } = string.Empty;
}

