using Anis.MembersManagment.Query.Abstractions.IRepositories;

namespace Anis.MembersManagment.Query.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IInvitationRepository Invitation { get; private set; }
        public ISubscriberRepository Subscriber { get; private set; }
        public IPermissionRepository Permission { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            Invitation = new InvitationRepository(_context);
            Subscriber = new SubscriberRepository(_context);
            Permission = new PermissionRepository(_context);
        }

        public async Task CommitAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
