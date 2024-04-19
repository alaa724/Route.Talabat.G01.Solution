using System.Text.Json;
using Talabat.Core.Entities;

namespace Talabat.Infrastructure.Data
{
	public static class StoreContextSeed
	{
		public async static Task SeedAsync(StoreDbContext _dbContext)
		{
			
			if(_dbContext.ProductBrands.Count() == 0)
			{
				var brandData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);


				if (brands?.Count() > 0)
				{
					foreach(var brand in brands)
					{
						_dbContext.Set<ProductBrand>().Add(brand);
					}
					await _dbContext.SaveChangesAsync();
				}
			}
			if (_dbContext.ProductCategories.Count() == 0)
			{

				var categoryData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/categories.json");
				var categories = JsonSerializer.Deserialize<List<ProductCategory>>(categoryData);


				if (categories?.Count() > 0)
				{
					foreach (var category in categories)
					{
						_dbContext.Set<ProductCategory>().Add(category);
					}
					await _dbContext.SaveChangesAsync();
				}
			}
			if (_dbContext.Products.Count() == 0)
			{

				var productData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/products.json");
				var products = JsonSerializer.Deserialize<List<Product>>(productData);


				if (products?.Count() > 0)
				{
					foreach (var product in products)
					{

						_dbContext.Set<Product>().Add(product);
					}
					await _dbContext.SaveChangesAsync();
				}
			}
		}
	}
}
