﻿using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Events;

namespace Anis.MembersManagment.Command.Domain
{
    public abstract class Aggregate<T> where T : Aggregate<T>, IAggregate
    {
        private readonly List<Event> _uncommittedEvents = [];

        public string Id { get; private set; } = string.Empty;
        public int Sequence { get; private set; }
        public int NextSequence => Sequence + 1;

        public IReadOnlyList<Event> GetUnCommittedEvents() => _uncommittedEvents;
        public void MarkChangesAsCommitted() => _uncommittedEvents.Clear();

        public static T LoadFromHistory(List<Event> history)
        {
            if (history.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(history), "history.Count == 0");

            var aggregate = CreateNewInstance();

            aggregate.ApplyPrevioslyCommittedChanges(history);

            return aggregate;
        }

        private static T CreateNewInstance()
        {
            var aggregate = (T?)Activator.CreateInstance(typeof(T),nonPublic: true);

            return aggregate ?? throw new InvalidOperationException("Aggregate creation failed");
        }

        private void ApplyPrevioslyCommittedChanges(List<Event> events) =>
            events.ForEach(@event => ValidateAndApplyChange(@event));

        private void ValidateAndApplyChange(Event @event)
        {
            SetIdAndSequence(@event);

            ValidateEvent(@event);

            Mutate(@event);
        }

        private void SetIdAndSequence(Event @event)
        {
            if (@event.Sequence == 1)
            {
                Id = @event.AggregateId;
            }

            Sequence++;
        }

        private void ValidateEvent(Event @event)
        {
            if (string.IsNullOrEmpty(Id))
                throw new InvalidOperationException("string.IsNullOrEmpty(Id)");

            if (@event.Sequence != Sequence)
                throw new InvalidOperationException("@event.Sequence != Sequence");
        }

        protected abstract void Mutate(Event @event);

        protected void ApplyNewChange(Event @event)
        {
            ValidateAndApplyChange(@event);

            _uncommittedEvents.Add(@event);
        }
    }
}
