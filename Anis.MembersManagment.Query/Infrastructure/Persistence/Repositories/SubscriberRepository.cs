using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Microsoft.EntityFrameworkCore;
using static Grpc.Core.Metadata;

namespace Anis.MembersManagment.Query.Infrastructure.Persistence.Repositories
{
    public class SubscriberRepository(ApplicationDbContext context) : BaseRepository<Subscriber>(context), ISubscriberRepository
    {
        public async Task ChangeStatusAsync(Subscriber entity)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.Id == entity.Id);
            subscriber?.ChangeStatus(entity);
        }

        public async Task UpdateSequence(string aggregateId, int sequence)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.Id == aggregateId);
            subscriber?.UpdateSequence(sequence);
        }
    }
}
