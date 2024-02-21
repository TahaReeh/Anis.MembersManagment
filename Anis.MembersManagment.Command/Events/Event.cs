namespace Anis.MembersManagment.Command.Events
{
    public abstract record Event(
        int Id,
        string AggregateId,
        int Sequence,
        DateTime DateTime,
        string UserId,
        int Version)
    {
        public string Type => GetType().Name;
    }

    public abstract record Event<T>(
        string AggregateId,
        int Sequence,
        DateTime DateTime,
        T Data,
        string UserId,
        int Version
        ) : Event(Id: default, AggregateId, Sequence, DateTime, UserId, Version);
}
