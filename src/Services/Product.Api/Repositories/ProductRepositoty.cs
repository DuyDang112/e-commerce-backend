using AutoMapper;
using Contract.Common.Interfaces;
using Infrastructures.Common;
using Microsoft.EntityFrameworkCore;
using Product.Api.Entity;
using Product.Api.Persistence;
using Product.Api.Repositories.Interfaces;
using Shared.DTOs.Product;
using Shared.SeedWork;

namespace Product.Api.Repositories
{
    public class ProductRepositoty : RepositoryBaseAsync<ProductCatalog, long, ProductContext>, IProductRepositry
    {
        private readonly ProductContext _context;
        private readonly IMapper _mapper;
        public ProductRepositoty(ProductContext context, IUnitOfWork<ProductContext> unitOfWork, IMapper mapper) : base(context, unitOfWork)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ProductCatalog> CreateProduct(ProductDto product)
        {
            var Product = _mapper.Map<ProductCatalog>(product);
            await CreateAsync(Product);
            if(product.CategoryIDs != null)
            {
                foreach (var id in product.CategoryIDs)
                {
                    _context.Add(new ProductCategoryProduct()
                    {
                        ProductCatalog = Product,
                        CategoryId = id
                    });
                }
            }
            return Product;
        }

        public async Task<PageList<ProductCatalog>> SearchProducts(GetProductPagingQueryDto query)
        {
            var products = _context.products.Include(pc => pc.ProductCategoryProducts)
                .AsQueryable();

            if(!string.IsNullOrEmpty(query.SearchTerm))
            {
                products = products.Where(p => p.Name.ToLower().Contains(query.SearchTerm));
            }

            if (query.Categories != null)
            {
                products = products.Where(p => p.ProductCategoryProducts.Where(pc => query.Categories.Contains(pc.CategoryId)).Any());
            }

            if(query.OrderBy == "decs")
            {
                products = products.OrderByDescending(p => p.Price);
            }


            var resut = await PageList<ProductCatalog>.ToPageListWithDatabaseRelation(products, query.PageNumber, query.PageSize);

            return resut;
        }

        public async Task<ProductCatalog> UpdateProduct(long id,ProductDto productDto)
        {
            var product = await GetByIdAsync(id, p => p.ProductCategoryProducts);

            if (product == null)
                return null;

          
            if (productDto.CategoryIDs == null)
                productDto.CategoryIDs = new long[] { };

            var oldCateIDs = product.ProductCategoryProducts.Select(c => c.CategoryId).ToArray();
            var newCateIDs = productDto.CategoryIDs;

            var removeCateProduct = from productCate in product.ProductCategoryProducts
                                    where (!newCateIDs.Contains(productCate.CategoryId))
                                    select productCate;

            _context.productCategories.RemoveRange(removeCateProduct);

            var addCateProducts = from CateId in newCateIDs
                                 where !oldCateIDs.Contains(CateId)
                                 select CateId;

            foreach (var CateId in addCateProducts)
            {
                _context.productCategories.Add(new ProductCategoryProduct()
                {
                    CategoryId = CateId,
                    ProductId = id
                });
            }

            var productUpdate = _mapper.Map(productDto, product); //MapperExtention

            await UpdateAsync(productUpdate);

            return productUpdate;
        }
    }
}
