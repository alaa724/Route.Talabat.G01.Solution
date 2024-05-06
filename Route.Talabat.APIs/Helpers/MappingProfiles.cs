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

			CreateMap<IdentityAddress, IdentityAddressDto>();

			CreateMap<AddressDto, Address>();



		}
	}
}
