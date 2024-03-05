using Anis.MembersManagment.Query.EventHandlers.Joined;
using Anis.MembersManagment.Query.EventHandlers.Removed;

namespace Anis.MembersManagment.Query.Test.Fakers.Removed
{
    public class MemberRemovedDataFaker : RecordFaker<MemberRemovedData>
    {
        public MemberRemovedDataFaker(MemberJoined memberJoined)
        {
            RuleFor(e => e.Id, memberJoined.AggregateId);
            RuleFor(e => e.AccountId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.SubscriptionId, memberJoined.Data.SubscriptionId);
            RuleFor(e => e.MemberId, memberJoined.Data.MemberId);
        }
    }
}
