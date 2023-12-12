using Contract.Common.Interfaces;
using Contract.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Common
{
    public class RepositoryBaseAsync<T, K, TContext> : RepositoryQueryBase<T,K,TContext> ,IRepositoryBaseAsync<T, K, TContext> 
        where T : EntityBase<K> 
        where TContext : DbContext
    {
        private readonly TContext _dbContext;
        private IUnitOfWork<TContext> _unitOfWork;

        public RepositoryBaseAsync(TContext dbContext, IUnitOfWork<TContext> unitOfWork) : base(dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void  Create(T entity) => _dbContext.Set<T>().AddAsync(entity);

        public async Task<K> CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await SaveChangesAsync();
            return entity.Id;
        }

        public IList<K>  CreateList(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
            return entities.Select(x => x.Id).ToList();
        }

        // khi thêm vào context có thể truy cập vào các tượng mới được thêm mặc dù chưa saveChange
        public async Task<List<K>> CreateListAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await SaveChangesAsync();
            return entities.Select(x => x.Id).ToList(); 
        }

        public  void Update(T entity)
        {
            if (_dbContext.Entry(entity).State == EntityState.Unchanged) return;

            T exist = _dbContext.Set<T>().Find(entity.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);
        }

        //use Entyry => optimize performent, Only update data fields change
        public async Task UpdateAsync(T entity)
        {
            //nếu không có gì thay dổi => completedTassk
            if(_dbContext.Entry(entity).State == EntityState.Unchanged) return;

            T exist = _dbContext.Set<T>().Find(entity.Id);
            _dbContext.Entry(exist).CurrentValues.SetValues(entity);

            await SaveChangesAsync();
            // return task.completed ( nếu  dùng async await thì không thể dùng return task.completed, => return;)
        }

        public void UpdateList(IEnumerable<T> entities) => _dbContext.Set<T>().AddRangeAsync(entities);

        public async Task UpdateListAsync(IEnumerable<T> entities)
        {
            await _dbContext.Set<T>().AddRangeAsync(entities);
            await SaveChangesAsync();
        }

        public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await SaveChangesAsync();
        }

        public void DeleteList(IEnumerable<T> entities) => _dbContext.Set<T>().RemoveRange(entities);

        public async Task DeleteListAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await SaveChangesAsync();
        }

        public Task<IDbContextTransaction> BeginTransactionAsync() =>
          _dbContext.Database.BeginTransactionAsync();

        public async Task EndTransactionAsync()
        {
            await SaveChangesAsync();
            await _dbContext.Database.CommitTransactionAsync();
        }

        public Task RollBackTransactionAsync() => _dbContext.Database.RollbackTransactionAsync();

        public Task<int> SaveChangesAsync() => _unitOfWork.CommitAsync();

     

     

     

      

       

   
    }
}
