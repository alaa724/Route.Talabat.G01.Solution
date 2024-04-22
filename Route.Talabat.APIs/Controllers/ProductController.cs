using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Route.Talabat.APIs.DTO;
using Route.Talabat.APIs.Errors;
using Route.Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecification;

namespace Route.Talabat.APIs.Controllers
{
	
	public class ProductController : BaseApiController
	{
		private readonly IGenericRepository<Product> _productsRepo;
		private readonly IGenericRepository<ProductBrand> _brandRepo;
		private readonly IGenericRepository<ProductCategory> _categoryRepo;
		private readonly IMapper _mapper;

		public ProductController(IGenericRepository<Product> productsRepo,
			IGenericRepository<ProductBrand> brandRepo ,
			IGenericRepository<ProductCategory> categoryRepo,
			IMapper mapper) 
		{
			_productsRepo = productsRepo;
			_brandRepo = brandRepo;
			_categoryRepo = categoryRepo;
			_mapper = mapper;
		}

		// api/Product
		[HttpGet]
		public async Task<ActionResult<Pagination<ProductToReturnDto>>> GetProducts([FromQuery] ProductSpecParams specParams)
		{
			var spec = new ProductWithBrandAndCategorySpecification(specParams);

			var products = await _productsRepo.GetAllWithSpecAsync(spec);

			var data = _mapper.Map<IReadOnlyList<Product> , IReadOnlyList<ProductToReturnDto>>(products);

			var countSpecs = new ProductWithFilterationForCountSpecification(specParams);

			var count = await _productsRepo.GetCountAsync(countSpecs);

			return Ok(new Pagination<ProductToReturnDto>(specParams.PageIndex ,specParams.PageSize , count ,  data));
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

		[HttpGet("brands")] // Get : api/Product/brands
		public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
		{
			var brands = await _brandRepo.GetAllAsync();
			return Ok(brands);
		}

		[HttpGet("categories")] // Get : api/Product/categories
		public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
		{
			var categories = await _categoryRepo.GetAllAsync();
			return Ok(categories);
		}
	}
}
