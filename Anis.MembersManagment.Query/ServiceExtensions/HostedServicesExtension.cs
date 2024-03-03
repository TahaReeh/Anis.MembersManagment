using Anis.MembersManagment.Query.Infrastructure.ServiceBus;

namespace Anis.MembersManagment.Query.ServiceExtensions
{
    public static class HostedServicesExtension
    {
        public static void AddHostedServices(this IServiceCollection services)
        {
            services.AddHostedService<MembersEventsListner>();
        }
    }
}
