using Anis.MembersManagment.Query.Abstractions.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace Anis.MembersManagment.Query.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T>(ApplicationDbContext context) : IBaseRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context = context;
        internal DbSet<T> dbSet = context.Set<T>();
        private static readonly char[] separator = [','];

        public async Task<IEnumerable<T>> GetAllAsync(
            Expression<Func<T, bool>>? filter = null,
            string? includeProperties = null,
            int? page = null,
            int? size = null
            )
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var property in includeProperties
                    .Split(separator, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            if (page is not null && size is not null)
            {
                query = query.Skip((page - 1) * size ?? 1).Take(size ?? 1);
            }

            return await query.ToListAsync();
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false)
        {
            IQueryable<T> query;
            if (tracked)
            {
                query = dbSet;
            }
            else
            {
                query = dbSet.AsNoTracking();
            }

            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                if (!string.IsNullOrEmpty(includeProperties))
                {
                    foreach (var property in includeProperties
                        .Split(separator, StringSplitOptions.RemoveEmptyEntries))
                    {
                        query = query.Include(property);
                    }
                }
            }

            return await query.FirstOrDefaultAsync();
        }

        public Task<bool> ExistsAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken) =>
            dbSet.AnyAsync(filter, cancellationToken);

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            await dbSet.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken)
        {
            await dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            return Task.CompletedTask;
        }

        public Task RemoveRangeAsync(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
            return Task.CompletedTask;
        }


    }
}
