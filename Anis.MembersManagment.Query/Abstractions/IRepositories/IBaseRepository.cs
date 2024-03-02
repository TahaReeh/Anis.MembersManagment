using System.Linq.Expressions;

namespace Anis.MembersManagment.Query.Abstractions.IRepositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);
        Task<IReadOnlyList<T>> GetPagedReponseAsync(int page, int size);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
        Task<T> AddAsync(T entity,CancellationToken cancellationToken);
        Task RemoveAsync(T entity);
        Task RemoveRangeAsync(IEnumerable<T> entities);
    }
}
