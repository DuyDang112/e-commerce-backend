using AutoMapper;
using Infrastructures.Mapping;
using Product.Api.Entity;
using Shared.DTOs.Product;

namespace Product.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ProductCatalog, ProductDto>().ReverseMap()
                .IgnoreAllNonExisting();
        }
    }
}
