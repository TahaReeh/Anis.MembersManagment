using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Microsoft.EntityFrameworkCore;
using static Grpc.Core.Metadata;

namespace Anis.MembersManagment.Query.Infrastructure.Persistence.Repositories
{
    public class InvitationRepository : BaseRepository<Invitation>, IInvitationRepository
    {
        public InvitationRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task ChangeStatusAsync(Invitation entity)
        {
            var invite = await _context.Invitations.FirstOrDefaultAsync(i => i.Id == entity.Id);
            if (invite is not null)
            {
                invite.ChangeStatus(entity);
            }
        }

        public async Task UpdateSequence(string aggregateId, int sequence)
        {
            var invite = await _context.Invitations.FirstOrDefaultAsync(i => i.Id == aggregateId);
            if (invite is not null)
            {
                invite.UpdateSequence(sequence);
            }
        }
    }
}
