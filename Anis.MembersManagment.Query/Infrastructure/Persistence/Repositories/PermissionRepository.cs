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
            p.UserId == entity.UserId && p.SubscriptionId == entity.SubscriptionId);
            if (permission is not null)
            {
                permission.ChangePermission(entity);
            }
        }
    }
}
