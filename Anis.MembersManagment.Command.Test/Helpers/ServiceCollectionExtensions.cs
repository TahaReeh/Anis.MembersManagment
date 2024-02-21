using Anis.MembersManagment.Command.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Anis.MembersManagment.Command.Test.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static void ReplaceWithInMemoryDatabase(this IServiceCollection services)
        {
            var ef = services.Single(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            services.Remove(ef);
            var dbName = Guid.NewGuid().ToString();
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase(dbName));
        }
    }
}
