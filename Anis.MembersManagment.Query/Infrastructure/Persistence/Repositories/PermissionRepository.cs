using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Anis.MembersManagment.Query.Entities;
using Microsoft.EntityFrameworkCore;

namespace Anis.MembersManagment.Query.Infrastructure.Persistence.Repositories
{
    public class PermissionRepository(ApplicationDbContext context) : BaseRepository<Permission>(context), IPermissionRepository
    {
        public async Task ChangePermissions(Permission entity)
        {
            var permission = await _context.Permissions.FirstOrDefaultAsync(p =>
            p.Id == entity.Id);
            permission?.ChangePermission(entity);
        }

        public async Task UpdateSequence(string aggregateId, int sequence)
        {
            var permission = await _context.Permissions.FirstOrDefaultAsync(p =>
            p.Id == aggregateId);
            permission?.UpdateSequence(sequence);
        }
    }
}
