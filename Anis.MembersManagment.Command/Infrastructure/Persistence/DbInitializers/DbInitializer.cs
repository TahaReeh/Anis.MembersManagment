
using Microsoft.EntityFrameworkCore;

namespace Anis.MembersManagment.Command.Infrastructure.Persistence.DbInitializers
{
    public class DbInitializer(ApplicationDbContext context) : IDbInitializer
    {
        private readonly ApplicationDbContext _context = context;

        public async Task InitializeAsync()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Any())
                {
                    await _context.Database.MigrateAsync();
                }
            }
            catch (Exception)
            {}
        }
    }
}
