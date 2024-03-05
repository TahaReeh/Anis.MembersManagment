using Anis.MembersManagment.Query.Entities;

namespace Anis.MembersManagment.Query.Abstractions.IRepositories
{
    public interface IPermissionRepository : IBaseRepository<Permission>
    {
        Task ChangePermissions(Permission entity);
        Task UpdateSequence(string aggregateId, int sequence);
    }
}
