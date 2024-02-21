using Anis.MembersManagment.Command.Events;
using Anis.MembersManagment.Command.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Anis.MembersManagment.Command.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new BaseEventConfigurations());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<InvitationSent, InvitationSentData>());
        }
    }
}
