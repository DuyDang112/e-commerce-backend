using Contract.Common.Interfaces;
using Product.Api.Entity;
using Product.Api.Persistence;
using Shared.DTOs.Product;
using Shared.SeedWork;

namespace Product.Api.Repositories.Interfaces
{
    public interface IProductRepositry : IRepositoryBaseAsync<ProductCatalog,long,ProductContext>
    {
        Task<PageList<ProductCatalog>> SearchProducts(GetProductPagingQueryDto query);
        Task<ProductCatalog> CreateProduct(ProductDto product);
        Task<ProductCatalog> UpdateProduct(long id,ProductDto product);
    }
}
