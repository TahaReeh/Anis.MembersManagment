using Anis.MembersManagment.Query.EventHandlers.Cancelled;
using Anis.MembersManagment.Query.EventHandlers.Sent;

namespace Anis.MembersManagment.Query.Test.Fakers.Cancelled
{
    public class InvitationCancelledDataFaker : RecordFaker<InvitationCancelledData>
    {
        public InvitationCancelledDataFaker(InvitationSent invitationSent)
        {
            RuleFor(e => e.Id, invitationSent.AggregateId);
            RuleFor(e => e.AccountId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.SubscriptionId, invitationSent.Data.SubscriptionId);
            RuleFor(e => e.MemberId, invitationSent.Data.MemberId);
        }
    }
}
