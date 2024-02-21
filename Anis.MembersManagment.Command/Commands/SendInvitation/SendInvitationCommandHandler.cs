using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Domain;
using MediatR;

namespace Anis.MembersManagment.Command.Commands.SendInvitation
{
    public class SendInvitationCommandHandler(IEventStore eventStore) : IRequestHandler<SendInvitationCommand, string>
    {
        private readonly IEventStore _eventStore = eventStore;

        public async Task<string> Handle(SendInvitationCommand command, CancellationToken cancellationToken)
        {
            int newinvitationNumber = 1;


            var invitation = Invitation.SendInvitation(command, newinvitationNumber.ToString());

            await _eventStore.CommitAsync(invitation, cancellationToken);

            return invitation.Id.ToString();
        }
    }
}
