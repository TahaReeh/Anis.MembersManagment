using Anis.MembersManagment.Command.Abstractions;
using Anis.MembersManagment.Command.Domain;
using Anis.MembersManagment.Command.Events;
using Microsoft.EntityFrameworkCore;

namespace Anis.MembersManagment.Command.Infrastructure.Persistence
{
    public class EventStore : IEventStore
    {
        private readonly ApplicationDbContext _context;

        public EventStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public Task<List<Event>> GetAllAsync(string aggregateId, CancellationToken cancellationToken) =>
            _context.Events
            .Where(x => x.AggregateId == aggregateId)
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);

        public Task<List<Event>> GetAllLikeAsync(string aggregateId, CancellationToken cancellationToken)=>
             _context.Events
            .Where(x => x.AggregateId.Contains(aggregateId))
            .OrderBy(x => x.Id)
            .ToListAsync(cancellationToken);


        public async Task CommitAsync(Invitation invitation, CancellationToken cancellationToken)
        {
            var events = invitation.GetUnCommittedEvents();

            await _context.Events.AddRangeAsync(events,cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
