namespace Anis.MembersManagment.Query.EventHandlers.Rejected
{
    public record class InvitationRejected(
        string AggregateId,
        int Sequence,
        InvitationRejectedData Data,
        DateTime DateTime,
        string UserId,
        int Version
        ) : Event<InvitationRejectedData>(
            AggregateId: AggregateId,
            Sequence: Sequence,
            Data: Data,
            DateTime: DateTime,
            UserId: UserId,
            Version: Version
            );
}
