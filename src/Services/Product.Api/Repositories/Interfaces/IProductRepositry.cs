using Contract.Common.Interfaces;
using Product.Api.Entity;
using Product.Api.Persistence;

namespace Product.Api.Repositories.Interfaces
{
    public interface IProductRepositry : IRepositoryBaseAsync<ProductCatalog,long,ProductContext>
    {

    }
}
