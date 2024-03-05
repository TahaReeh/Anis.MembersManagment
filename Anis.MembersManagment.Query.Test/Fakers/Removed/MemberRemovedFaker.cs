using Anis.MembersManagment.Query.EventHandlers.Joined;
using Anis.MembersManagment.Query.EventHandlers.Removed;

namespace Anis.MembersManagment.Query.Test.Fakers.Removed
{
    public class MemberRemovedFaker : EventFaker<MemberRemoved, MemberRemovedData>
    {
        public MemberRemovedFaker(MemberJoined memberJoined)
        {
            RuleFor(e => e.AggregateId, memberJoined.AggregateId);
            RuleFor(e => e.Sequence, memberJoined.Sequence + 1);
            RuleFor(e => e.Data,new MemberRemovedDataFaker(memberJoined));
        }
    }
}
