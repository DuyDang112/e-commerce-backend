using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Product.Api.Entity;
using Product.Api.Repositories;
using Shared.DTOs.Product;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Product.Api.Repositories.Interfaces;

namespace Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _categoryRepository.FindAll()
                                .Include(c => c.CategoryChildren)
                                .Include(c => c.ParentCategory)
                                .AsQueryable()
                                .Where(c => c.ParentCategory == null).ToListAsync();
            var result = _mapper.Map<IEnumerable<CategoryDto>>(categories);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetCategory([Required] long id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null) return NotFound();

            var result = _mapper.Map<CategoryDto>(category);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            var category = _mapper.Map<ProductCategory>(categoryDto);
            category.No = Guid.NewGuid().ToString();
            await _categoryRepository.CreateAsync(category);
            var result = _mapper.Map<CategoryDto>(category);
            return Ok(result);
        }

        //use MapperExtention for updateProduct
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateCategory([Required] long id, [FromBody] CategoryDto categoryDto)
        {
            var category = await _categoryRepository.UpdateCategory(id,categoryDto);
            if (category == null)
                return NotFound();

            var result = _mapper.Map<CategoryDto>(category);
            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteCategory([Required] long id)
        {
            var category = await _categoryRepository.DeleteCategory(id);
            if (category == null)
                return NotFound();
            

            return NoContent();
        }
    }
}
