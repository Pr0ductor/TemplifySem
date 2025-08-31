using MediatR;

namespace Templify.Application.Features.UserSettings.Commands
{
    public class UpdateUserProfileCommandHandler : IRequestHandler<UpdateUserProfileCommand, bool>
    {
        public Task<bool> Handle(UpdateUserProfileCommand request, CancellationToken cancellationToken)
        {
            // Реализация
            return Task.FromResult(true);
        }
    }
}











