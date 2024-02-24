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
            var events = await _eventStore.GetAllAsync($"{command.SubscriptionId}_{command.MemberId}", cancellationToken);

            Member member;

            if (events.Count != 0)
                member = Member.LoadFromHistory(events);
            else
                member = new();

            member.SendInvitation(command);

            await _eventStore.CommitAsync(member, cancellationToken);

            return member.Id;
        }
    }
}
