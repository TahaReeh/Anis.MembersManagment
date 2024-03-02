using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Microsoft.EntityFrameworkCore;

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
            if (subscriber != null)
            {
                subscriber.ChangeStatus(entity);
            }
        }
    }
}
