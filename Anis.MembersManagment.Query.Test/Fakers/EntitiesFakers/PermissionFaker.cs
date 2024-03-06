using Anis.MembersManagment.Query.Entities;
using Anis.MembersManagment.Query.EventHandlers.Joined;

namespace Anis.MembersManagment.Query.Test.Fakers.EntitiesFakers
{
    public class PermissionFaker : RecordFaker<Permission>
    {
        public PermissionFaker()
        {
            RuleFor(p => p.Id, faker => faker.Random.Guid().ToString());
            RuleFor(p => p.Sequence, faker => faker.Random.Int(1, 9));
            RuleFor(p => p.UserId, faker => faker.Random.Guid().ToString());
            RuleFor(p => p.SubscriptionId, faker => faker.Random.Guid().ToString());
            RuleFor(p => p.Transfer, faker => faker.Random.Bool());
            RuleFor(p => p.PurchaseCards, faker => faker.Random.Bool());
            RuleFor(p => p.ManageDevices, faker => faker.Random.Bool());
        }

        public Permission WithKnownAggregate(string aggregateId) =>
            RuleFor(p => p.Id, aggregateId);

    }
}
