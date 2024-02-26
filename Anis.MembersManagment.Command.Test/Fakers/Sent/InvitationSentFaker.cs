using Anis.MembersManagment.Command.Events;

namespace Anis.MembersManagment.Command.Test.Fakers.Sent
{
    public class InvitationSentFaker : EventFaker<InvitationSent, InvitationSentData>
    {
        readonly InvitationSentDataFaker fakerData = new();

        public InvitationSentFaker()
        {
            RuleFor(e => e.AggregateId, $"{fakerData.Generate().SubscriptionId}_{fakerData.Generate().MemberId}");
            RuleFor(e => e.Sequence, 1);
            RuleFor(e => e.Data, fakerData);
            
        }
    }
}
