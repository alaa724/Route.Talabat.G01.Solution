using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Route.Talabat.APIs.DTO;
using Route.Talabat.APIs.Errors;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Route.Talabat.APIs.Controllers
{

	public class OrdersController : BaseApiController
	{
		private readonly IOrderService _orderService;
		private readonly IMapper _mapper;

		public OrdersController(
			IOrderService orderService,
			IMapper mapper
			)
		{
			_orderService = orderService;
			_mapper = mapper;
		}

		[ProducesResponseType(typeof(Order) , StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status400BadRequest)]
		[HttpPost] // Post : /api/orders
		public async Task<ActionResult<Order>> CreateOrder(OrderDto orderDto)
		{
			var address = _mapper.Map<AddressDto, Address>(orderDto.ShippingAddress);

			var order = await _orderService.CreateOrderAsync(orderDto.BuyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, address);

			if (order is null) return BadRequest(new ApiResponse(400));

			return Ok(order);
		}

		[HttpGet] // Get : /api/Orders?email=alaahamdy@gmail.com
		public async Task<ActionResult<IReadOnlyList<Order>>> GetOrderForUser(string email)
		{
			var orders = await _orderService.GetOrderForUserAsync(email);

			return Ok(orders);
		}


		[HttpGet("{id}")] // Get : /api/orders/1?email=alaahamdy@gmail.com
		public async Task<ActionResult<Order>> GetOrdersForUser(int id , string email)
		{
			var order = await _orderService.GetOrderByIdForUserAsync(id , email);

			if (order is null) return NotFound(new ApiResponse(404));

			return Ok(order);
		}

	}
}
