using Anis.MembersManagment.Query.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anis.MembersManagment.Query.Infrastructure.Persistence.Configurations
{
    public class SubscriberConfigurations : IEntityTypeConfiguration<Subscriber>
    {
        public void Configure(EntityTypeBuilder<Subscriber> builder)
        {
            builder.Property(x=>x.Sequence).IsConcurrencyToken();
        }
    }
}
