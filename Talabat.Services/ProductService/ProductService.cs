using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.ProductSpecification;

namespace Talabat.Application.ProductService
{
	public class ProductService : IProductService
	{
		private readonly IUniteOfWork _uniteOfWork;

		public ProductService(IUniteOfWork uniteOfWork)
		{
			_uniteOfWork = uniteOfWork;
		}

		public async Task<IReadOnlyList<Product>> GetProductsAsync(ProductSpecParams specParams)
		{
			var spec = new ProductWithBrandAndCategorySpecification(specParams);

			var products = await _uniteOfWork.Repository<Product>().GetAllWithSpecAsync(spec);

			return products;
		}

		public async Task<Product?> GetProductAsync(int productId)
		{
			var spec = new ProductWithBrandAndCategorySpecification(productId);

			var product = await _uniteOfWork.Repository<Product>().GetWithSpecAsync(spec);

			return product;
		}
		public async Task<int> GetCountAsync(ProductSpecParams specParams)
		{
			var countSpecs = new ProductWithFilterationForCountSpecification(specParams);

			var count = await _uniteOfWork.Repository<Product>().GetCountAsync(countSpecs);

			return count;
		}
		public async Task<IReadOnlyList<ProductBrand>> GetBrandsAsync()
			=> await _uniteOfWork.Repository<ProductBrand>().GetAllAsync();


		public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync()
			=> await _uniteOfWork.Repository<ProductCategory>().GetAllAsync();

		
	}
}
