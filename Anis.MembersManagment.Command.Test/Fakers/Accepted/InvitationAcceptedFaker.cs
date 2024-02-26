using Anis.MembersManagment.Command.Events;

namespace Anis.MembersManagment.Command.Test.Fakers.Accepted
{
    public class InvitationAcceptedFaker : EventFaker<InvitationAccepted, InvitationAcceptedData>
    {
        public InvitationAcceptedFaker()
        {
            RuleFor(e => e.Sequence, 1);
        }

        public InvitationAcceptedFaker For(InvitationSent invitationSent)
        {
            RuleFor(e => e.AggregateId, invitationSent.AggregateId);
            RuleFor(e => e.UserId, invitationSent.UserId);
            RuleFor(e => e.Sequence, invitationSent.Sequence + 1);
            RuleFor(e => e.DateTime, invitationSent.DateTime.AddMinutes(1));
            return this;
        }
    }
}
