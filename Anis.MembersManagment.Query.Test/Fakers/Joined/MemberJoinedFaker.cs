using Anis.MembersManagment.Query.EventHandlers.Joined;

namespace Anis.MembersManagment.Query.Test.Fakers.Joined
{
    public class MemberJoinedFaker : EventFaker<MemberJoined, MemberJoinedData>
    {
        public MemberJoinedFaker(int sequence)
        {
            RuleFor(e => e.Sequence, sequence);
            RuleFor(e => e.Data, new MemberJoinedDataFaker());
        }
    }
}
