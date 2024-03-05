using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anis.MembersManagment.Query.Infrastructure.Persistence.Repositories
{
    public class PermissionRepository : BaseRepository<Permission>, IPermissionRepository
    {
        public PermissionRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task ChangePermissions(Permission entity)
        {
            var permission = await _context.Permissions.FirstOrDefaultAsync(p =>
            p.Id == entity.Id);
            if (permission is not null)
            {
                permission.ChangePermission(entity);
            }
        }

        public async Task UpdateSequence(string aggregateId, int sequence)
        {
            var permission = await _context.Permissions.FirstOrDefaultAsync(p =>
            p.Id == aggregateId);
            if (permission is not null)
            {
                permission.UpdateSequence(sequence);
            }
        }
    }
}
