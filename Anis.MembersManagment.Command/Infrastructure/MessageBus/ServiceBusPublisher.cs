using Anis.MembersManagment.Command.Infrastructure.Persistence;
using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;

namespace Anis.MembersManagment.Command.Infrastructure.MessageBus
{
    public class ServiceBusPublisher(ServiceBusClient client, IServiceProvider provider)
    {
        private readonly ServiceBusSender _sender = client.CreateSender("taha-member-managment");
        private readonly IServiceProvider _provider = provider;

        public void StartPublishing()
        {
            PublishEvents().GetAwaiter().GetResult();
        }

        private async Task PublishEvents()
        {
            while (true)
            {
                using var scope = _provider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var messages = await dbContext.OutboxMessages
                    .Include(x => x.Event)
                    .OrderBy(x => x.Id)
                    .Take(100)
                    .ToListAsync();

                if (messages.Count == 0) return;

                foreach (var message in messages)
                {
                    if (message.Event is null)
                    {
                        throw new InvalidOperationException("Event is null, please include the event in the query");
                    }

                    var json = JsonSerializer.Serialize(message.Event);

                    var serviceBusMessage = new ServiceBusMessage(Encoding.UTF8.GetBytes(json))
                    {
                        PartitionKey = message.Event.AggregateId.ToString(),
                        SessionId = message.Event.AggregateId.ToString(),
                        Subject = message.Event.Type
                    };

                    await _sender.SendMessageAsync(serviceBusMessage);

                    dbContext.OutboxMessages.Remove(message);
                    await dbContext.SaveChangesAsync();
                }
            }
        }
    }
}
