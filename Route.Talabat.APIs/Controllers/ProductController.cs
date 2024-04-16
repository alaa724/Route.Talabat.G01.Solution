using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;

namespace Route.Talabat.APIs.Controllers
{
	
	public class ProductController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productsRepo;

		public ProductController(IGenericRepository<Product> productsRepo) 
		{
			_productsRepo = productsRepo;
		}

		// api/Product
		[HttpGet]
		public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
		{
			var products = await _productsRepo.GetAllAsync();

			return Ok(products);
		}

		// api/Product/1
		[HttpGet("{id}")]
		public async Task<ActionResult<Product>> GetProduct(int id)
		{
			var product = await _productsRepo.GetAsync(id);

			if (product is null)
				return NotFound();
			return Ok(product);
		}
	}
}
