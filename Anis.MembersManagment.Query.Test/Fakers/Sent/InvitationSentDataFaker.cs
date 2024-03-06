using Anis.MembersManagment.Query.Entities;
using Anis.MembersManagment.Query.EventHandlers.Sent;
using Bogus;

namespace Anis.MembersManagment.Query.Test.Fakers.Sent
{
    public class InvitationSentDataFaker : RecordFaker<InvitationSentData>
    {
        private readonly string SubscriptionId;
        private readonly string MemberId;

        public InvitationSentDataFaker()
        {
            SubscriptionId = Guid.NewGuid().ToString();
            MemberId = Guid.NewGuid().ToString();

            RuleFor(e=>e.AccountId,faker=>faker.Random.Guid().ToString());
            RuleFor(e=>e.SubscriptionId,faker=>faker.Random.Guid().ToString());
            RuleFor(e=>e.MemberId,faker=>faker.Random.Guid().ToString());
            RuleFor(e => e.Permissions, () => new PermissionFaker()
            .GenerateWithKnownAggregate($"{SubscriptionId}_{MemberId}"));
        }
    }
}
