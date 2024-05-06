using System.ComponentModel.DataAnnotations;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Route.Talabat.APIs.DTO
{
	public class OrderDto : BaseEntity
	{
        [Required]
        public string BuyerEmail { get; set; }

		[Required]
		public string BasketId { get; set; }

		[Required]
		public int DeliveryMethodId { get; set; }

		public AddressDto ShippingAddress { get; set; }
    }
}
