using Contract.Common.Interfaces;
using Infrastructures.Common;
using Product.Api.Entity;
using Product.Api.Persistence;
using Product.Api.Repositories.Interfaces;

namespace Product.Api.Repositories
{
    public class ProductRepositoty : RepositoryBaseAsync<ProductCatalog, long, ProductContext>, IProductRepositry
    {
        public ProductRepositoty(ProductContext context, IUnitOfWork<ProductContext> unitOfWork) : base(context, unitOfWork)
        {
        }
    }
}
