using Contract.Common.Interfaces;
using Contract.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Common
{
    public class RepositoryQueryBase<T, K, TContext> : IRepositoryQueryBase<T, K, TContext>
        where T : EntityBase<K>
        where TContext : DbContext
    {
        private readonly TContext _dbContext;

        public RepositoryQueryBase(TContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IQueryable<T> FindAll(bool trackChanges = false) =>
            !trackChanges ? _dbContext.Set<T>().AsNoTracking() :
            _dbContext.Set<T>();

        public IQueryable<T> FindAll(bool trackChanges = false, params Expression<Func<T, object>>[] incluProperties)
        {
            var item = FindAll(trackChanges);
            item = incluProperties.Aggregate(item, (current, includeProperty) => current.Include(includeProperty));
            return item;
        }

        //AsNotracking => các bản ghi được trả về từ truy vấn sẽ không bị theo dõi để ghi lại thay đổi.
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false) =>
            !trackChanges
            ? _dbContext.Set<T>().Where(expression).AsNoTracking()
            : _dbContext.Set<T>();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false, params Expression<Func<T, object>>[] incluProperties)
        {
            var item = FindByCondition(expression, trackChanges);
            item = incluProperties.Aggregate(item, (current, includeProperty) => current.Include(includeProperty));
            return item;
        }

        public async Task<T?> GetByIdAsync(K id) =>
            await FindByCondition(x => x.Id.Equals(id))
            .FirstOrDefaultAsync();

        public async Task<T?> GetByIdAsync(K id, params Expression<Func<T, object>>[] incluProperties) =>
             await FindByCondition(x => x.Id.Equals(id), false, incluProperties)
            .FirstOrDefaultAsync();
    }
}
