using Microsoft.EntityFrameworkCore;
using Payslip.Core.Repositories.Base;
using Payslip.Infrastructure.Data;
using System.Linq.Expressions;

namespace Payslip.Infrastructure.Repositories.Base
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly DataContext Context;
        private readonly DbSet<TEntity> _dbSet;

        public Repository(DataContext context)
        {
            this.Context = context;
            _dbSet = Context.Set<TEntity>();
        }
        public async Task AddAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public async Task<ICollection<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Context.Set<TEntity>().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetWithPagingAsync(int pageSize, int pageIndex)
        {
            return await Context.Set<TEntity>().Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToListAsync();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().ToList();
        }

        public IEnumerable<TEntity> AsEnumerable()
        {
            return Context.Set<TEntity>().AsEnumerable();
        }

        public ValueTask<TEntity> GetByIdAsync(Guid id)
        {
            return Context.Set<TEntity>().FindAsync(id);
        }

        public void Remove(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().SingleOrDefaultAsync(predicate);
        }

        public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Context.Set<TEntity>().FirstOrDefaultAsync(predicate);
        }

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().FirstOrDefault(predicate);
        }

        public virtual IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> dbQuery = Context.Set<TEntity>();
            var query = dbQuery.Where(where);
            return query;
        }

        public virtual IQueryable<TEntity> OrderByDescending(Expression<Func<TEntity, object>> orderByDescending)
        {
            IQueryable<TEntity> dbQuery = Context.Set<TEntity>();
            var query = dbQuery.OrderByDescending(orderByDescending);
            return query;
        }

        public virtual IQueryable<TEntity> OrderBy(Expression<Func<TEntity, object>> orderBy)
        {
            IQueryable<TEntity> dbQuery = Context.Set<TEntity>();
            var query = dbQuery.OrderBy(orderBy);
            return query;
        }

        public IEnumerable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeExpressions)
        {
            return includeExpressions.Aggregate(_dbSet.AsQueryable(), (query, path) => query.Include(path));
        }

        public virtual bool Any(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> dbQuery = Context.Set<TEntity>();

            if (where != null)
                return dbQuery.Any(where);

            return dbQuery.Any();
        }

        public async virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>> where)
        {
            IQueryable<TEntity> dbQuery = Context.Set<TEntity>();

            if (where != null)
                return await dbQuery.AnyAsync(where);

            return await dbQuery.AnyAsync();
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public virtual async Task<IEnumerable<TEntity>> GetListWithIncludeAsync(string includeProperties,
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public virtual async Task<TEntity> GetFirstWithIncludeAsync(Expression<Func<TEntity, bool>>? filter = null, params Expression<Func<TEntity, object>>[] includeProperty)
        {
            IQueryable<TEntity> query = _dbSet;

            //Apply eager loading
            foreach (Expression<Func<TEntity, object>> navigationProperty in includeProperty)
                query = query.Include<TEntity, object>(navigationProperty);

            if (filter is not null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public int GetTotal()
        {
            return Context.Set<TEntity>().Count();
        }
    }
}
