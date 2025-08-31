namespace Templify.Application.Common.DTOs
{
    public record ProductPurchaseDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public int ProductId { get; init; }
        public DateTime PurchaseDate { get; init; }
        public decimal Price { get; init; }
        public string Status { get; init; } = string.Empty;
        
        // Навигационные свойства
        public string UserName { get; init; } = string.Empty;
        public string UserEmail { get; init; } = string.Empty;
        public string UserAvatarUrl { get; init; } = string.Empty;
        public string ProductName { get; init; } = string.Empty;
        public string ProductImageUrl { get; init; } = string.Empty;
        public string ProductCategory { get; init; } = string.Empty;
        public string AuthorName { get; init; } = string.Empty;
        public string AuthorAvatarUrl { get; init; } = string.Empty;
    }
}
