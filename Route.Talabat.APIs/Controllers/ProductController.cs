using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Route.Talabat.APIs.DTO;
using Route.Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecification;

namespace Route.Talabat.APIs.Controllers
{
	
	public class ProductController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productsRepo;
		private readonly IMapper _mapper;

		public ProductController(IGenericRepository<Product> productsRepo , IMapper mapper) 
		{
			_productsRepo = productsRepo;
			_mapper = mapper;
		}

		// api/Product
		[HttpGet]
		public async Task<ActionResult<IEnumerable<ProductToReturnDto>>> GetProducts()
		{
			var spec = new ProductWithBrandAndCategorySpecification();

			var products = await _productsRepo.GetAllWithSpecAsync(spec);

			var mapedProducts = _mapper.Map<IEnumerable<Product> , IEnumerable<ProductToReturnDto>>(products);

			return Ok(mapedProducts);
		}


		[ProducesResponseType(typeof(ProductToReturnDto) , StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status404NotFound)]
		// api/Product/1
		[HttpGet("{id}")]
		public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
		{
			var spec = new ProductWithBrandAndCategorySpecification(id);

			var product = await _productsRepo.GetWithSpecAsync(spec);


			if (product is null)
				return NotFound(new ApiResponse(404));

			var mapedProduct = _mapper.Map<Product, ProductToReturnDto>(product);

			return Ok(mapedProduct);
		}
	}
}
