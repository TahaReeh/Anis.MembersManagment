using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.Abstractions.IRepositories
{
    public interface ISubscriberRepository : IBaseRepository<Subscriber>
    {
        Task ChangeStatusAsync(Subscriber entity);
    }
}
