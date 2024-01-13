using Contract.Common.Interfaces;
using Product.Api.Entity;
using Product.Api.Persistence;
using Shared.DTOs.Product;

namespace Product.Api.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepositoryBaseAsync<ProductCategory, long, ProductContext>
    {
        Task<string> DeleteCategory(long id);
        Task<ProductCategory> UpdateCategory(long id,CategoryDto category);
    }
}
