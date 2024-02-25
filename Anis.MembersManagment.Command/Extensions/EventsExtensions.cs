using Anis.MembersManagment.Command.Commands.AcceptInvitation;
using Anis.MembersManagment.Command.Commands.CancelInvitation;
using Anis.MembersManagment.Command.Commands.JoinMember;
using Anis.MembersManagment.Command.Commands.RejectInvitation;
using Anis.MembersManagment.Command.Commands.SendInvitation;
using Anis.MembersManagment.Command.Events;

namespace Anis.MembersManagment.Command.Extensions
{
    public static class EventsExtensions
    {

        #region Invitation
        public static InvitationSent ToEvent(this SendInvitationCommand command, int sequence) => new(
         AggregateId: $"{command.SubscriptionId}_{command.MemberId}",
         Sequence: sequence,
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

        public static InvitationAccepted ToEvent(this AcceptInvitationCommand command, int sequence) => new(
          AggregateId: command.Id,
           Sequence: sequence,
           DateTime: DateTime.UtcNow,
           Data: new InvitationAcceptedData(
               AccountId: command.AccountId,
               SubscriptionId: command.SubscriptionId,
               MemberId: command.MemberId),
           UserId: command.UserId,
           Version: 1
           );

        public static InvitationCancelled ToEvent(this CancelInvitationCommand command, int sequence) => new(
           AggregateId: command.Id,
           Sequence: sequence,
           DateTime: DateTime.UtcNow,
           Data: new InvitationCancelledData(
               AccountId: command.AccountId,
               SubscriptionId: command.SubscriptionId,
               MemberId: command.MemberId),
           UserId: command.UserId,
           Version: 1
           );

        public static InvitationRejected ToEvent(this RejectInvitationCommand command, int sequence) => new(
           AggregateId: command.Id,
           Sequence: sequence,
           DateTime: DateTime.UtcNow,
           Data: new InvitationRejectedData(
               AccountId: command.AccountId,
               SubscriptionId: command.SubscriptionId,
               MemberId: command.MemberId),
           UserId: command.UserId,
           Version: 1
           );

        #endregion

        #region Member

        public static MemberJoined ToEvent(this JoinMemberCommand command, int sequence) => new(
           AggregateId: $"{command.SubscriptionId}_{command.MemberId}",
           Sequence: sequence,
           DateTime: DateTime.UtcNow,
           Data: new MemberJoinedData(
               AccountId: command.AccountId,
               SubscriptionId: command.SubscriptionId,
               MemberId: command.MemberId,
               Permissions: command.Permissions
               ),
           UserId: command.UserId,
           Version: 1
           );

        #endregion
    }
}
