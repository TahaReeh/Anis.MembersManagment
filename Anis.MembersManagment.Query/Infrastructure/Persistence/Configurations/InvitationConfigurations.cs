using Anis.MembersManagment.Query.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Anis.MembersManagment.Query.Infrastructure.Persistence.Configurations
{
    public class InvitationConfigurations : IEntityTypeConfiguration<Invitation>
    {
        public void Configure(EntityTypeBuilder<Invitation> builder)
        {
            builder.Property(x=>x.Sequence).IsConcurrencyToken();
        }
    }
}
