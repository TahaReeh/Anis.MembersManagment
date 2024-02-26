using Anis.MembersManagment.Command.Events;

namespace Anis.MembersManagment.Command.Test.Fakers.Sent
{
    public class InvitationSentDataFaker : RecordFaker<InvitationSentData>
    {
        public InvitationSentDataFaker()
        {
            RuleFor(e => e.AccountId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.SubscriptionId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.MemberId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.Permissions.Transfer, faker => faker.Random.Bool());
            RuleFor(e => e.Permissions.PurchaseCards, faker => faker.Random.Bool());
            RuleFor(e => e.Permissions.ManageDevices, faker => faker.Random.Bool());
        }
    }
}
