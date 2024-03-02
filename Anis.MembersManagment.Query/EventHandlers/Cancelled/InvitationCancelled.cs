namespace Anis.MembersManagment.Query.EventHandlers.Cancelled
{
    public record class InvitationCancelled(
        string AggregateId,
        int Sequence,
        InvitationCancelledData Data,
        DateTime DateTime,
        string UserId,
        int Version
        ) : Event<InvitationCancelledData>(
            AggregateId: AggregateId,
            Sequence: Sequence,
            Data: Data,
            DateTime: DateTime,
            UserId: UserId,
            Version: Version
            );
}
