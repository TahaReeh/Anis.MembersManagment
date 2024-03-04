using Anis.MembersManagment.Query.Entities;
using Anis.MembersManagment.Query.EventHandlers.Sent;

namespace Anis.MembersManagment.Query.Test.Fakers.Sent
{
    public class InvitationSentFaker : EventFaker<InvitationSent, InvitationSentData>
    {
        public InvitationSentFaker(int sequence)
        {
            RuleFor(e => e.Sequence, sequence);
            RuleFor(e=>e.Data, new InvitationSentDataFaker());
        }


    }
}
