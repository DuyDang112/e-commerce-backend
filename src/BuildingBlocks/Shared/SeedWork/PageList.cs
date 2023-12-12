using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.SeedWork
{
    public class PageList<T> : List<T>
    {
        public PageList(IEnumerable<T> items, long totalItems, int pageNumber, int pageSize) 
        {
            _metaData = new MetaData
            {
                TotalItems = totalItems,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
            };

            AddRange(items);
        }

        private MetaData _metaData { get; }

        public MetaData GetMetaData()
        {
            return _metaData;
        }

        public static async Task<PageList<T>> ToPageList(IMongoCollection<T> source,
            FilterDefinition<T> filter, int pageIndex, int pageSize)
        {
            var count = await source.Find(filter).CountDocumentsAsync();
            var item = await source.Find(filter)
                .Skip((pageIndex - 1) * pageSize)
                .Limit(pageSize)
                .ToListAsync();

            return new PageList<T>(item, count, pageIndex, pageSize);
        }

        //public static async Task<PageList<T>> ToPageList(IQueryable<T> source,
        //  int pageIndex, int pageSize)
        //{
        //    var count = await source.CountAsync();
        //    var item = await source
        //        .Skip((pageIndex - 1) * pageSize)
        //        .Take(pageSize).ToListAsync();

        //    return new PageList<T>(item, count, pageIndex, pageSize);
        //}
    }
}
