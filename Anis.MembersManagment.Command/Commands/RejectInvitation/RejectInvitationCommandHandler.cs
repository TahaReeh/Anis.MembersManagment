using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Domain;
using Anis.MembersManagment.Command.Exceptions;
using Grpc.Core;
using MediatR;

namespace Anis.MembersManagment.Command.Commands.RejectInvitation
{
    public class RejectInvitationCommandHandler(IEventStore eventStore) : IRequestHandler<RejectInvitationCommand, string>
    {
        private readonly IEventStore _eventStore = eventStore;

        public async Task<string> Handle(RejectInvitationCommand command, CancellationToken cancellationToken)
        {
            var events = await _eventStore.GetAllAsync(command.Id, cancellationToken);

            if (events.Count == 0)
                throw new NotFoundException("Invitation not found");

            var member = Member.LoadFromHistory(events);

            member.RejectInvitation(command);

            await _eventStore.CommitAsync(member, cancellationToken);

            return command.Id;
        }
    }
}
