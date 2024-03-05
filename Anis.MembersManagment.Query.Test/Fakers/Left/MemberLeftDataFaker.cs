using Anis.MembersManagment.Query.EventHandlers.Joined;
using Anis.MembersManagment.Query.EventHandlers.Left;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.MembersManagment.Query.Test.Fakers.Left
{
    public class MemberLeftDataFaker : RecordFaker<MemberLeftData>
    {
        public MemberLeftDataFaker(MemberJoined memberJoined)
        {
            RuleFor(e => e.Id, memberJoined.AggregateId);
            RuleFor(e => e.AccountId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.SubscriptionId, memberJoined.Data.SubscriptionId);
            RuleFor(e => e.MemberId, memberJoined.Data.MemberId);
        }
    }
}
