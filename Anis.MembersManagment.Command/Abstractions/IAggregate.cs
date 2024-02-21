using Anis.MembersManagment.Command.Events;

namespace Anis.MembersManagment.Command.Abstractions
{
    public interface IAggregate
    {
        string Id {  get; }
        int Sequence { get; }
        IReadOnlyList<Event> GetUnCommittedEvents();
        void MarkChangesAsCommitted();
    }
}
