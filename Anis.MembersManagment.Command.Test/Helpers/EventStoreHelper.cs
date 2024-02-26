using Anis.MembersManagment.Command.Events;
using Bogus;

namespace Anis.MembersManagment.Command.Test.Helpers
{
    public class EventStoreHelper
    {
        private readonly IServiceProvider _provider;

        public EventStoreHelper(IServiceProvider provider)
        {
            _provider = provider;
        }

        //public async Task<TEvent> GenerateAndCommitAsync<TEvent>(Faker<TEvent> faker) where TEvent : Event
        //{
        //    var eventStore = _provider.GetRequiredService<IEventStore>();
        //    var @event = faker.Generate();
        //    await eventStore.CommitAsync(@event,new CancellationToken());
        //    return @event;
        //}
    }
}
