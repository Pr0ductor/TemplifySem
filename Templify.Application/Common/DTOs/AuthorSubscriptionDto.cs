namespace Templify.Application.Common.DTOs
{
    public record AuthorSubscriptionDto
    {
        public int Id { get; init; }
        public int UserId { get; init; }
        public int AuthorId { get; init; }
        public DateTime SubscriptionDate { get; init; }
        public bool IsActive { get; init; }
        
        // Навигационные свойства
        public string UserName { get; init; } = string.Empty;
        public string UserEmail { get; init; } = string.Empty;
        public string UserAvatarUrl { get; init; } = string.Empty;
        public string AuthorName { get; init; } = string.Empty;
        public string AuthorEmail { get; init; } = string.Empty;
        public string AuthorAvatarUrl { get; init; } = string.Empty;
        public string AuthorSpecialization { get; init; } = string.Empty;
    }
}
