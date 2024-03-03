using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Infrastructure.Persistence;
using Anis.MembersManagment.Query.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Anis.MembersManagment.Query.ServiceExtensions
{
    public static class EntityFrameworkExtension
    {
        public static void AddEntityFramework(this IServiceCollection services,IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Database");

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseSqlServer(connectionString));

            services.AddScoped<IUnitOfWork,UnitOfWork>();
        }
    }
}
