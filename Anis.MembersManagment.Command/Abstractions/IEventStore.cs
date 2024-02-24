using Anis.MembersManagment.Command.Domain;
using Anis.MembersManagment.Command.Events;

namespace Anis.MembersManagment.Command.Abstractions
{
    public interface IEventStore
    {
        Task<List<Event>> GetAllAsync(string aggregateId, CancellationToken cancellationToken);
        Task<List<Event>> GetAllLikeAsync(string aggregateId, CancellationToken cancellationToken);
        Task CommitAsync(Member member, CancellationToken cancellationToken);
    }
}
