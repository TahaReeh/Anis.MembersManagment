namespace Anis.MembersManagment.Query.Infrastructure.ServiceBus
{
    public class ServiceBusOptions
    {
        public const string ServiceBus = "ServiceBus";
        public string? TopicName { get; set; }
        public string? SubscriptionName { get; set; }
    }
}
