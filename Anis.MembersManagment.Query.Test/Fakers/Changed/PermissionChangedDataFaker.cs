using Anis.MembersManagment.Query.EventHandlers.Changed;
using Anis.MembersManagment.Query.EventHandlers.Joined;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.MembersManagment.Query.Test.Fakers.Changed
{
    public class PermissionChangedDataFaker : RecordFaker<PermissionChangedData>
    {
        public PermissionChangedDataFaker(MemberJoined memberJoined)
        {
            RuleFor(e => e.Id, memberJoined.AggregateId);
            RuleFor(e => e.AccountId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.SubscriptionId, memberJoined.Data.SubscriptionId);
            RuleFor(e => e.MemberId, memberJoined.Data.MemberId);
            RuleFor(e => e.Permissions, () => new PermissionFaker()
            .GenerateWithKnownAggregate(memberJoined.AggregateId));
        }
    }
}
