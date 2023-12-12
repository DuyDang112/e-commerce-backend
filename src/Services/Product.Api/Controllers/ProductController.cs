using AutoMapper;
using Contract.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Api.Entity;
using Product.Api.Persistence;
using Product.Api.Repositories.Interfaces;
using Shared.DTOs.Product;
using System.ComponentModel.DataAnnotations;

namespace Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepositry _repository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepositry repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<IActionResult> GetProDucts()
        {
            var products = await _repository.FindAll().ToListAsync();
            var result = _mapper.Map<IEnumerable<ProductDto>>(products);
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetProDuct([Required] long id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null) return NotFound();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] ProductDto productDto)
        {
            var product = _mapper.Map<ProductCatalog>(productDto);
            await _repository.CreateAsync(product);
            await _repository.SaveChangesAsync();
            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        //use MapperExtention for updateProduct
        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateProduct([Required] long id, [FromBody] ProductDto productDto)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            var updateProduct = _mapper.Map(productDto, product); //MapperExtention
            await _repository.UpdateAsync(updateProduct);
            await _repository.SaveChangesAsync();

            var result = _mapper.Map<ProductDto>(product);
            return Ok(result);
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteProduct([Required] long id)
        {
            var product = await _repository.GetByIdAsync(id);
            if (product == null)
                return NotFound();

            await _repository.DeleteAsync(product);
            await _repository.SaveChangesAsync();

            return NoContent();
        }


    }
}
