﻿using Anis.MembersManagment.Query.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Anis.MembersManagment.Query.Test.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static void ReplaceWithInMemoryDatabase(this IServiceCollection services)
        {
            var ef = services.Single(d => d.ServiceType == typeof(DbContextOptions<QueryApplicationDbContext>));
            services.Remove(ef);
            var dbName = Guid.NewGuid().ToString();
            services.AddDbContext<QueryApplicationDbContext>(options => options.UseInMemoryDatabase(dbName));
        }
    }
}