using Anis.MembersManagment.Query.Entities;
using Anis.MembersManagment.Query.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Anis.MembersManagment.Query.Infrastructure.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Subscriber> Subscribers { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new InvitationConfigurations());
            modelBuilder.ApplyConfiguration(new SubscriberConfigurations());
            modelBuilder.ApplyConfiguration(new PermissionConfigurations());
        }
    }
}
