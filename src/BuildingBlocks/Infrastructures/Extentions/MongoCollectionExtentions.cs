using MongoDB.Driver;
using Shared.SeedWork;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Extentions
{
    public static class MongoCollectionExtentions
    {
        public static Task<PageList<TDestination>> PaginatedListAsync<TDestination>(
            this IMongoCollection<TDestination> collection,
            FilterDefinition<TDestination> filter,
            int pageIndex, int pageSize) where TDestination : class
            => PageList<TDestination>.ToPageList(collection, filter, pageIndex, pageSize);
    }
}
