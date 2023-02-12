using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using ShopAPI.DTO.Mapping.Resolvers;

namespace ShopAPI.DTO.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(p => p.ProductType, src => src.MapFrom(c => c.ProductType.Name))
                .ForMember(p => p.ProductBrand, src => src.MapFrom(c => c.ProductBrand.Name))
                .ForMember(p => p.PictureUrl, src => src.MapFrom<ProductUrlResolver>());

            // map both sides ReverseMap
            CreateMap<Address, AddressDTO>().ReverseMap();
        }
    }
}
