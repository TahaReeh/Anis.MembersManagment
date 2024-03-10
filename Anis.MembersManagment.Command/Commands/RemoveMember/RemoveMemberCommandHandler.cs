using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Domain;
using Anis.MembersManagment.Command.Exceptions;
using Grpc.Core;
using MediatR;

namespace Anis.MembersManagment.Command.Commands.RemoveMember
{
    public class RemoveMemberCommandHandler(IEventStore eventStore) : IRequestHandler<RemoveMemberCommand, string>
    {
        private readonly IEventStore _eventStore = eventStore;

        public async Task<string> Handle(RemoveMemberCommand command, CancellationToken cancellationToken)
        {
            var events = await _eventStore.GetAllAsync(command.Id, cancellationToken);

            if (events.Count == 0)
                throw new NotFoundException("There is no such member in this subscription");

            var member = Member.LoadFromHistory(events);

            member.RemoveMember(command);

            await _eventStore.CommitAsync(member, cancellationToken);   

            return command.Id;
        }
    }
}
