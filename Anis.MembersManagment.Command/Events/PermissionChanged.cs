using Anis.MembersManagment.Command.Domain;

namespace Anis.MembersManagment.Command.Events
{
    public record PermissionChanged(
        string AggregateId,
        int Sequence,
        DateTime DateTime,
        PermissionChangedData Data,
        string UserId,
        int Version
        ) : Event<PermissionChangedData>(AggregateId, Sequence, DateTime, Data, UserId, Version);

    public record PermissionChangedData(
        string AccountId,
        string SubscriptionId,
        string MemberId,
        Permission Permissions
        );
}
