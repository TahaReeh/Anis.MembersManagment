using Anis.MembersManagment.Command.Events;
using Anis.MembersManagment.Command.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Anis.MembersManagment.Command.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<OutboxMessage> OutboxMessages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OutboxMessageConfigurations());
            modelBuilder.ApplyConfiguration(new BaseEventConfigurations());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<InvitationSent, InvitationSentData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<InvitationAccepted, InvitationAcceptedData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<InvitationCancelled, InvitationCancelledData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<InvitationRejected, InvitationRejectedData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<MemberJoined, MemberJoinedData>());
            modelBuilder.ApplyConfiguration(new GenericEventConfiguration<MemberRemoved, MemberRemovedData>());
        }
    }
}
