using Azure.Messaging.ServiceBus;

namespace Anis.MembersManagment.Query.Infrastructure.ServiceBus
{
    public class MembersServiceBus(IConfiguration configuration)
    {
        public ServiceBusClient Client { get; } = new ServiceBusClient(configuration["ServiceBus"]);
    }
}
