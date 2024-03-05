using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Microsoft.EntityFrameworkCore;
using static Grpc.Core.Metadata;

namespace Anis.MembersManagment.Query.Infrastructure.Persistence.Repositories
{
    public class SubscriberRepository : BaseRepository<Subscriber>, ISubscriberRepository
    {
        public SubscriberRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task ChangeStatusAsync(Subscriber entity)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.Id == entity.Id);
            if (subscriber is not null)
            {
                subscriber.ChangeStatus(entity);
            }
        }

        public async Task IncrementSequenceAsync(Subscriber entity)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.Id == entity.Id);
            if (subscriber is not null)
            {
                subscriber.IncrementSequence();
            }
        }

        public async Task UpdateSequence(string aggregateId, int sequence)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(s => s.Id == aggregateId);
            if (subscriber is not null)
            {
                subscriber.UpdateSequence(sequence);
            }
        }
    }
}
