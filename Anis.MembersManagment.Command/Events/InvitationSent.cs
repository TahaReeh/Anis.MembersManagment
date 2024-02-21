using Anis.MembersManagment.Command.Domain;

namespace Anis.MembersManagment.Command.Events
{
    public record InvitationSent(
        string AggregateId,
        int Sequence,
        DateTime DateTime,
        InvitationSentData Data,
        string UserId,
        int Version
        ) : Event<InvitationSentData>(AggregateId, Sequence, DateTime, Data, UserId, Version);

    public record InvitationSentData(
        string AccountId,
        string SubscriptionId,
        string MemberId,
        Permission Permissions
        );

}