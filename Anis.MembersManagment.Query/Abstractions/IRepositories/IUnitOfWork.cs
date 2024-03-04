namespace Anis.MembersManagment.Query.Abstractions.IRepositories
{
    public interface IUnitOfWork
    {
        IInvitationRepository Invitation {  get; }
        ISubscriberRepository Subscriber { get; }
        IPermissionRepository Permission { get; }

        Task CommitAsync(CancellationToken cancellationToken);
    }
}
