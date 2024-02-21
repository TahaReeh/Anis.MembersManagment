using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Commands.AcceptInvitation;
using Anis.MembersManagment.Command.Commands.SendInvitation;
using Anis.MembersManagment.Command.Events;
using Anis.MembersManagment.Command.Extensions;

namespace Anis.MembersManagment.Command.Domain
{
    public class Invitation : Aggregate<Invitation>, IAggregate
    {

        public static Invitation SendInvitation(SendInvitationCommand command, string newinvitationNumber)
        {
            var invitation = new Invitation();
            invitation.ApplyNewChange(command.ToEvent(newinvitationNumber));
            return invitation;
        }

        public void AcceptInvitation(AcceptInvitationCommand command)
        {
            ApplyNewChange(command.ToEvent(NextSequence));
        }

        public string AccountId { get; private set; } = string.Empty;
        public string SubscriptionId { get; private set; } = string.Empty;
        public string MemberId { get; private set; } = string.Empty;

        protected override void Mutate(Event @event)
        {
            switch (@event)
            {
                case InvitationSent e:
                    Mutate(e);
                    break;
                case InvitationAccepted e:
                    Mutate(e);
                    break;
            }
        }

        public void Mutate(InvitationSent @event)
        {
            AccountId = @event.Data.AccountId;
            SubscriptionId = @event.Data.SubscriptionId;
            MemberId = @event.Data.MemberId;
        }

        public void Mutate(InvitationAccepted @event)
        {
            AccountId = @event.Data.AccountId;
            SubscriptionId = @event.Data.SubscriptionId;
            MemberId = @event.Data.MemberId;
        }
    }
}
