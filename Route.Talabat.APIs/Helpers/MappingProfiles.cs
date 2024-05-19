using AutoMapper;
using Route.Talabat.APIs.DTO;
using Route.Talabat.APIs.DTO.IdentityDto;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Entities.Order_Aggregate;
using Address = Talabat.Core.Entities.Order_Aggregate.Address;
using IdentityAddressDto = Route.Talabat.APIs.DTO.IdentityDto.AddressDto;
using IdentityAddress = Talabat.Core.Entities.Identity.Address;
using AddressDto = Route.Talabat.APIs.DTO.AddressDto;

namespace Route.Talabat.APIs.Helpers
{
	public class MappingProfiles : Profile
	{
		public MappingProfiles()
		{
			CreateMap<Product, ProductToReturnDto>()
				.ForMember(P => P.Brand, O => O.MapFrom(S => S.Brand.Name))
				.ForMember(P => P.Category, O => O.MapFrom(S => S.Category.Name))
				.ForMember(P => P.PictureUrl , O => O.MapFrom<ProductPictureUrlResolver>());

			CreateMap<CustomerBasketDto, CustomerBasket>();
			CreateMap<BasketItemDto, BasketItem>();

			CreateMap<IdentityAddress, IdentityAddressDto>().ReverseMap();

			CreateMap<AddressDto, Address>();

			CreateMap<Order, OrderToReturnDto>()
				.ForMember(d => d.DeliveryMethod, O => O.MapFrom(S => S.DeliveryMethod.ShortName))
				.ForMember(d => d.DeliveryMethodCost , O => O.MapFrom(S => S.DeliveryMethod.Cost));

			CreateMap<OrderItem, OrderItemDto>()
				.ForMember(d => d.ProductId, O => O.MapFrom(S => S.Product.ProductId))
				.ForMember(d => d.ProductName , O => O.MapFrom(S => S.Product.ProductName))
				.ForMember(d => d.ProductUrl , O => O.MapFrom(S => S.Product.ProductUrl))
				.ForMember(d => d.ProductUrl , O => O.MapFrom<OrderItemPictureUrlResolver>());



		}
	}
}
