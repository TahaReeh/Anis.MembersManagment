using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Domain;
using Anis.MembersManagment.Command.Exceptions;
using MediatR;

namespace Anis.MembersManagment.Command.Commands.AcceptInvitation
{
    public class AcceptInvitationCommandHandler(IEventStore eventStore) : IRequestHandler<AcceptInvitationCommand, string>
    {
        private readonly IEventStore _eventStore = eventStore;

        public async Task<string> Handle(AcceptInvitationCommand command, CancellationToken cancellationToken)
        {
            var events = await _eventStore.GetAllAsync(command.Id, cancellationToken);

            if (events.Count == 0)
                throw new NotFoundException("Invitation not found");

           

            var invitation = Invitation.LoadFromHistory(events);

            invitation.AcceptInvitation(command);

            await _eventStore.CommitAsync(invitation, cancellationToken);

            return command.Id;
        }
    }
}
