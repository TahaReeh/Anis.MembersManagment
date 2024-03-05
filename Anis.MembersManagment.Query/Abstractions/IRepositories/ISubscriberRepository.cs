using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.Abstractions.IRepositories
{
    public interface ISubscriberRepository : IBaseRepository<Subscriber>
    {
        Task ChangeStatusAsync(Subscriber entity);
        Task IncrementSequenceAsync(Subscriber entity);
        Task UpdateSequence(string aggregateId, int sequence);
    }
}
