using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.EventHandlers.Changed
{
    public record PermissionChangedData(
        string Id,
        string AccountId,
        string SubscriptionId,
        string MemberId,
        Permission Permissions
        );
}
