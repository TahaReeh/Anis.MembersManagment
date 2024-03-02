using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.Abstractions.IRepositories
{
    public interface IInvitationRepository : IBaseRepository<Invitation>
    {
        Task ChangeStatusAsync(Invitation entity);
    }
}
