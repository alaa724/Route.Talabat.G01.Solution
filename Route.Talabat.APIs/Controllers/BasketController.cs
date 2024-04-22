using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Route.Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;

namespace Route.Talabat.APIs.Controllers
{

	public class BasketController : BaseApiController
	{
		private readonly IBasketRepository _basketRepo;

		public BasketController(IBasketRepository basketRepo)
		{
			_basketRepo = basketRepo;
		}

		[HttpGet] // Get : api/Basket?id=
		public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
		{
			var basket = await _basketRepo.GetBasketAsync(id);
			return Ok(basket ?? new CustomerBasket(id)); 
		}

		[HttpPost] // Post : /api/Basket?id=
		public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
		{
			var createdOrUpdatedBasket = await _basketRepo.UpdateBasketAsync(basket);

			if (createdOrUpdatedBasket is null)
				return BadRequest(new ApiResponse(400));

			return Ok(createdOrUpdatedBasket);
		}

		[HttpDelete] // Delete : /api/Basket?id=
		public async Task<ActionResult<bool>> DeleteBasket(string id)
		{
			return await _basketRepo.DeleteBasketAsync(id);
		}
	}
}
