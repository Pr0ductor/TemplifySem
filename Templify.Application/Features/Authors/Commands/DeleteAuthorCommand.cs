using MediatR;

namespace Templify.Application.Features.Authors.Commands
{
    public record DeleteAuthorCommand : IRequest<bool>
    {
        public int Id { get; init; }
    }
}
