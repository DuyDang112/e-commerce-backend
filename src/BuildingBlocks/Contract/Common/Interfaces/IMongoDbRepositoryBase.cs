using Contract.Domain;
using MongoDB.Driver;

namespace Contract.Common.Interfaces
{
    public interface IMongoDbRepositoryBase<T> where T : MongoEntity
    {
        IMongoCollection<T> FindAll(ReadPreference? readPreference = null);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(string id);
    }
}
