using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using ShopAPI.DTO.Mapping.Resolvers;
using OA = Core.Entities.OrderAggregate;

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
            CreateMap<CustomerBasketDTO, CustomerBasket>();
            CreateMap<BasketItemDTO, BasketItem>();

            CreateMap<AddressDTO, OA.Address>();
            CreateMap<OA.Order, OrderToReturnDTO>()
                .ForMember(p => p.DeliveryMethod, src => src.MapFrom(c => c.DeliveryMethod.ShortName))
                .ForMember(p => p.ShippingPrice, src => src.MapFrom(c => c.DeliveryMethod.Price));
            CreateMap<OA.OrderItem, OrderItemDTO>()
                .ForMember(p => p.ProductId, src => src.MapFrom(c => c.ItemOrdered.ProductItemId))
                .ForMember(p => p.ProductName, src => src.MapFrom(c => c.ItemOrdered.ProductName))
                .ForMember(p => p.PictureUrl, src => src.MapFrom<OrderItemUrlResolver>());
        }
    }
}
