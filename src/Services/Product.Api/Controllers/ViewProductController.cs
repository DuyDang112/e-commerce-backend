using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Api.Repositories.Interfaces;
using Shared.DTOs.Product;

namespace Product.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ViewProductController : ControllerBase
    {
        private readonly IProductRepositry _repository;
        private readonly IMapper _mapper;

        public ViewProductController(IProductRepositry repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Route("getbysearchterm")]
        [HttpGet]
        public async Task<IActionResult> GetProDuctBySearchterm([FromQuery] GetProductPagingQueryDto query)
        {
            var result = await _repository.SearchProducts(query);
            return Ok(result);
        }
    }
}
