
using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Domain;
using Grpc.Core;
using MediatR;

namespace Anis.MembersManagment.Command.Commands.Leave
{
    public class LeaveCommandHandler(IEventStore eventStore) : IRequestHandler<LeaveCommand, string>
    {
        private readonly IEventStore _eventStore = eventStore;

        public async Task<string> Handle(LeaveCommand command, CancellationToken cancellationToken)
        {
            var events = await _eventStore.GetAllAsync(command.Id, cancellationToken);

            if (events.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "You are not a member of this subscription"));

            var member = Member.LoadFromHistory(events);

            member.Leave(command);

            await _eventStore.CommitAsync(member, cancellationToken);

            return command.Id;
        }
    }
}
