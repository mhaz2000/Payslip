using System.Linq.Expressions;

namespace Payslip.Core.Repositories.Base
{
    public interface IRepository<TEntity> where TEntity : class
    {
        int GetTotal();
        ValueTask<TEntity> GetByIdAsync(Guid id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<IEnumerable<TEntity>> GetWithPagingAsync(int pageSize, int pageIndex);
        IEnumerable<TEntity> GetAll();
        IEnumerable<TEntity> AsEnumerable();
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);
        Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicat);
        Task AddAsync(TEntity entity);
        Task AddRangeAsync(IEnumerable<TEntity> entities);
        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
        IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> where);
        IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeExpressions);
        IQueryable<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderByDescending);
        IQueryable<TEntity> OrderBy(Expression<Func<TEntity, object>> orderBy);
        bool Any(Expression<Func<TEntity, bool>> where);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where);
        Task<IEnumerable<TEntity>> GetListWithIncludeAsync(string includeProperties, Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);
        IQueryable<TEntity> AsQueryable();
        Task<TEntity> GetFirstWithIncludeAsync(Expression<Func<TEntity, bool>> filter = null, params Expression<Func<TEntity, object>>[] includeProperty);
    }
}
