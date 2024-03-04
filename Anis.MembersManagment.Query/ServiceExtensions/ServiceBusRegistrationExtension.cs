using Anis.MembersManagment.Query.Infrastructure.ServiceBus;

namespace Anis.MembersManagment.Query.ServiceExtensions
{
    public static class ServiceBusRegistrationExtension
    {
        public static void AddServiceBus(this IServiceCollection services)
        {
            services.AddSingleton<MembersServiceBus>();
        }
    }
}
