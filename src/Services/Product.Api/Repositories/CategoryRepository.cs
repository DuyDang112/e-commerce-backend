using AutoMapper;
using Contract.Common.Interfaces;
using Infrastructures.Common;
using Microsoft.EntityFrameworkCore;
using Product.Api.Entity;
using Product.Api.Persistence;
using Product.Api.Repositories.Interfaces;
using Shared.DTOs.Product;

namespace Product.Api.Repositories
{
    public class CategoryRepository : RepositoryBaseAsync<ProductCategory, long, ProductContext>, ICategoryRepository
    {
        private readonly IMapper _mapper;
        private readonly ProductContext _context;
        public CategoryRepository(ProductContext dbContext, IUnitOfWork<ProductContext> unitOfWork, IMapper mapper) : base(dbContext, unitOfWork)
        {
            _mapper = mapper;
            _context = dbContext;
        }

        public async Task<string> DeleteCategory(long id)
        {
            var category = await GetByIdAsync(id, c => c.CategoryChildren);
            if (category == null) return null;

            _context.Attach(category);
            foreach (var cCategory in category.CategoryChildren)
            {
                cCategory.ParentCategoryId = category.ParentCategoryId;
                _context.Entry(cCategory).State = EntityState.Modified;
            }


            await DeleteAsync(category);

            return $"{id}";

        }

        public async Task<ProductCategory> UpdateCategory(long id, CategoryDto categoryDto)
        {
            var category = await GetByIdAsync(id);
            if (category == null) return null;

            bool canUpdate = true;

            if (category.ParentCategoryId == category.Id)
            {
                canUpdate = false;
            }

            // Kiem tra thiet lap muc cha phu hop
            if (canUpdate && category.ParentCategoryId != null)
            {
                var childCates = FindByCondition(c => c.ParentCategoryId == category.Id, false, c => c.CategoryChildren)
                            .ToList();


                // Func check Id 
                Func<List<ProductCategory>, bool> checkCateIds = null;
                checkCateIds = (cates) =>
                {
                    foreach (var cate in cates)
                    {
                        if (cate.Id == categoryDto.ParentCategoryId)
                        {
                            canUpdate = false;
                            return true;
                        }
                        if (cate.CategoryChildren != null)
                            return checkCateIds(cate.CategoryChildren.ToList());

                    }
                    return false;
                };
                // End Func 
                checkCateIds(childCates);
            }

            if (canUpdate)
            {
                if (categoryDto.ParentCategoryId == -1)
                {
                    categoryDto.ParentCategoryId = null;
                }

                var updateCategory = _mapper.Map(categoryDto, category); //MapperExtention
                await UpdateAsync(updateCategory);
                return updateCategory;
            }

            return category;
        }
    }
}
