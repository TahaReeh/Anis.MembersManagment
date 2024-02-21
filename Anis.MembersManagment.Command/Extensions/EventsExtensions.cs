using Anis.MembersManagment.Command.Commands.SendInvitation;
using Anis.MembersManagment.Command.Events;

namespace Anis.MembersManagment.Command.Extensions
{
    public static class EventsExtensions
    {
        public static InvitationSent ToEvent(this SendInvitationCommand command, string newinvitationNumber) => new(
         AggregateId: $"{command.SubscriptionId}_{command.MemberId}_{newinvitationNumber}",
         Sequence: 1,
         DateTime: DateTime.UtcNow,
         Data: new InvitationSentData(
             AccountId: command.AccountId,
             SubscriptionId: command.SubscriptionId,
             MemberId: command.MemberId,
             Permissions: command.Permissions
             ),
         UserId: command.UserId,
         Version: 1
         );
    }
}
