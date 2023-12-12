using AutoMapper;
using Inventoty.Product.Api.Entities;
using Shared.DTOs.Inventory;

namespace Inventoty.Product.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<InventotyEntry, InventoryEntryDto>().ReverseMap();
            CreateMap<PurchaseProductDto, InventoryEntryDto>();
        }
    }
}
