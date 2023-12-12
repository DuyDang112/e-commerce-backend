using Contract.Common.Interfaces;
using Contract.Domain;
using Infrastructures.Extentions;
using MongoDB.Driver;
using Shared.Configurations;
using System.Linq.Expressions;

namespace Infrastructures.Common
{
    public class MongoDbRepositoryBase<T> : IMongoDbRepositoryBase<T> where T : MongoEntity
    {
        private IMongoDatabase Database { get; }
        public MongoDbRepositoryBase(IMongoClient mongoClient, MongoDbSettings mongoDbSettings)
        {
            Database = mongoClient.GetDatabase(mongoDbSettings.DatabaseName);
        }

        public IMongoCollection<T> FindAll(ReadPreference? readPreference = null) =>
            Database.WithReadPreference(readPreference ?? ReadPreference.Primary)
            .GetCollection<T>(GetCollectionName());

        protected virtual IMongoCollection<T> Collection =>
            Database.GetCollection<T>(GetCollectionName());

        public Task CreateAsync(T entity) => Collection.InsertOneAsync(entity);

        public Task UpdateAsync(T entity)
        {
            Expression<Func<T, string>> func = f => f.Id;
            var value = (string)entity.GetType()
                .GetProperty(func.Body.ToString()
                .Split(".")[1])?.GetValue(entity,null);
            var filter = Builders<T>.Filter.Eq(func, value);

            return Collection.ReplaceOneAsync(filter, entity);
        }

        public Task DeleteAsync(string id) => Collection.DeleteOneAsync(x => x.Id.Equals(id));
   
        private static string GetCollectionName()
        {
            return (typeof(T).GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                .FirstOrDefault() as BsonCollectionAttribute)?.CollectionName;
        }
    }
}
