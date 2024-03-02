namespace Anis.MembersManagment.Query.EventHandlers.Accepted
{
    public record class InvitationAccepted(
        string AggregateId,
        int Sequence,
        InvitationAcceptedData Data,
        DateTime DateTime,
        string UserId,
        int Version
        ) : Event<InvitationAcceptedData>(
            AggregateId: AggregateId,
            Sequence: Sequence,
            Data: Data,
            DateTime: DateTime,
            UserId: UserId,
            Version: Version
            );
}
