using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Domain;
using MediatR;

namespace Anis.MembersManagment.Command.Commands.JoinMember
{
    public class JoinMemberCommandHandler : IRequestHandler<JoinMemberCommand, string>
    {
        private readonly IEventStore _eventStore;

        public JoinMemberCommandHandler(IEventStore eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<string> Handle(JoinMemberCommand command, CancellationToken cancellationToken)
        {
            var events = await _eventStore.GetAllAsync($"{command.SubscriptionId}_{command.MemberId}",cancellationToken);

            Member member;

            if (events.Count != 0)
                member = Member.LoadFromHistory(events);
            else
                member = new();

            member.JoinMember(command);

            await _eventStore.CommitAsync(member, cancellationToken);   

            return member.Id;
 
        }
    }
}
