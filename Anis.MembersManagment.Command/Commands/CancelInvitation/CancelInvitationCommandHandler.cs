using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Domain;
using Anis.MembersManagment.Command.Exceptions;
using Grpc.Core;
using MediatR;

namespace Anis.MembersManagment.Command.Commands.CancelInvitation
{
    public class CancelInvitationCommandHandler(IEventStore eventStore) : IRequestHandler<CancelInvitationCommand, string>
    {
        private readonly IEventStore _eventStore = eventStore;

        public async Task<string> Handle(CancelInvitationCommand command, CancellationToken cancellationToken)
        {
            var events = await _eventStore.GetAllAsync(command.Id, cancellationToken);

            if (events.Count == 0)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invitation not found"));

            if (events.Last().Type is "InvitationAccepted") //or "MemberJoined"
                throw new RpcException(new Status(StatusCode.InvalidArgument, "The member already exists in this subscription"));

            if (events.Last().Type is "InvitationCancelled" or "InvitationRejected")
                throw new RpcException(new Status(StatusCode.InvalidArgument, "This Invitation is invalid"));

            var invitation = Invitation.LoadFromHistory(events);

            invitation.CancelInvitation(command);

            await _eventStore.CommitAsync(invitation, cancellationToken);

            return command.Id;
        }
    }
}
