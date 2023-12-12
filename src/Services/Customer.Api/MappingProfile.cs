using AutoMapper;
using Customer.Api.Entities;
using Infrastructures.Mapping;
using Shared.DTOs.Customer;

namespace Customer.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.Customer, CustomerDto>().ReverseMap()
                .IgnoreAllNonExisting();
        }
    }
}
