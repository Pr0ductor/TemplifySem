namespace Templify.Application.Common.DTOs
{
    public record AuthorCreateDto
    {
        public string DisplayName { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string AvatarUrl { get; init; } = string.Empty;
        public string Specialization { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public string? UserId { get; init; } // Может быть null для сидовых авторов
    }
}
