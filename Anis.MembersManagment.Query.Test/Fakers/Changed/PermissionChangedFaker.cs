using Anis.MembersManagment.Query.EventHandlers.Changed;
using Anis.MembersManagment.Query.EventHandlers.Joined;

namespace Anis.MembersManagment.Query.Test.Fakers.Changed
{
    public class PermissionChangedFaker : EventFaker<PermissionChanged, PermissionChangedData>
    {
        public PermissionChangedFaker(MemberJoined memberJoined)
        {
            RuleFor(e => e.AggregateId, memberJoined.AggregateId);
            RuleFor(e => e.Sequence, memberJoined.Sequence + 1);
            RuleFor(e => e.Data, new PermissionChangedDataFaker(memberJoined));
        }
    }
}
