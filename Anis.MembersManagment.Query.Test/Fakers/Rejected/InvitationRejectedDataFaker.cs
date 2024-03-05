using Anis.MembersManagment.Query.EventHandlers.Rejected;
using Anis.MembersManagment.Query.EventHandlers.Sent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.MembersManagment.Query.Test.Fakers.Rejected
{
    public class InvitationRejectedDataFaker : RecordFaker<InvitationRejectedData>
    {
        public InvitationRejectedDataFaker(InvitationSent invitationSent)
        {
            RuleFor(e => e.Id, invitationSent.AggregateId);
            RuleFor(e => e.AccountId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.SubscriptionId, invitationSent.Data.SubscriptionId);
            RuleFor(e => e.MemberId, invitationSent.Data.MemberId);
        }
    }
}
