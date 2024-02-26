using Anis.MembersManagment.Command.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anis.MembersManagment.Command.Test.Fakers
{
    public abstract class EventFaker<TEvent,TData> : RecordFaker<TEvent>
        where TEvent : Event<TData>
    {
        protected EventFaker()
        {
            RuleFor(e => e.AggregateId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.Sequence, faker => faker.Random.Int());
            RuleFor(e => e.UserId, faker => faker.Random.Guid().ToString());
            RuleFor(e => e.DateTime, faker => DateTime.UtcNow);
            RuleFor(e => e.Version, 1);
        }
    }
}
