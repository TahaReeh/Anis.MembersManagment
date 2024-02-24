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

            var member = Member.LoadFromHistory(events);

            member.CancelInvitation(command);

            await _eventStore.CommitAsync(member, cancellationToken);

            return command.Id;
        }
    }
}
