using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.EventHandlers.Joined
{
    public record MemberJoinedData(
    string AccountId,
    string SubscriptionId,
    string MemberId,
    Permission Permissions
    );
}
