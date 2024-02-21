namespace Anis.MembersManagment.Command.Infrastructure.Persistence.DbInitializers
{
    public interface IDbInitializer
    {
        Task InitializeAsync();
    }
}
