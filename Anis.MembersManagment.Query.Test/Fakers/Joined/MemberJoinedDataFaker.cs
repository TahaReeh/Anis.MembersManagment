using Anis.MembersManagment.Query.EventHandlers.Joined;

namespace Anis.MembersManagment.Query.Test.Fakers.Joined
{
    public class MemberJoinedDataFaker : RecordFaker<MemberJoinedData>
    {
        public MemberJoinedDataFaker()
        {
            RuleFor(e => e.AccountId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.SubscriptionId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.MemberId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.Permissions, () => new PermissionFaker());
        }
    }
}
