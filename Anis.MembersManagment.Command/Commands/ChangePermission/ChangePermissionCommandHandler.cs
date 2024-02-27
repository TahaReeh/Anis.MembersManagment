using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Domain;
using Grpc.Core;
using MediatR;

namespace Anis.MembersManagment.Command.Commands.ChangePermission
{
    public class ChangePermissionCommandHandler : IRequestHandler<ChangePermissionCommand, string>
    {
        private readonly IEventStore _eventStore;

        public ChangePermissionCommandHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<string> Handle(ChangePermissionCommand command, CancellationToken cancellationToken)
        {
            var events = await _eventStore.GetAllAsync(command.Id, cancellationToken);

            if (events.Count == 0)
                throw new RpcException(new Status(StatusCode.NotFound, "There is no such member in this subscription"));

            var member = Member.LoadFromHistory(events);

            member.ChangePermission(command);

            await _eventStore.CommitAsync(member, cancellationToken);

            return command.Id;
        }
    }
}
