namespace Anis.MembersManagment.Query.EventHandlers.Joined
{
    public record class MemberJoined(
        string AggregateId,
        int Sequence,
        MemberJoinedData Data,
        DateTime DateTime,
        string UserId,
        int Version
        ) : Event<MemberJoinedData>(
            AggregateId:AggregateId,
            Sequence:Sequence,
            Data : Data,
            DateTime:DateTime,
            UserId:UserId,
            Version:Version
            );
}
