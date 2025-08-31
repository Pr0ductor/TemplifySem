using Templify.Domain.Enums;

namespace Templify.Application.Common.DTOs;

public class ProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Author { get; set; } = string.Empty;
    public int AuthorId { get; set; }
    public string AuthorAvatarUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public CategoryType CategoryType { get; set; }
    public int Downloads { get; set; }
    public string? Tags { get; set; }
}

