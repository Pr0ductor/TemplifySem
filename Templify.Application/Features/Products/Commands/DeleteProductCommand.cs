using MediatR;

namespace Templify.Application.Features.Products.Commands;

public record DeleteProductCommand : IRequest<bool>
{
    public int ProductId { get; init; }
    public string UserId { get; init; } = string.Empty;
    public bool IsAdmin { get; init; } = false;
}

