using System.Text.Json;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Infrastructure.Data
{
	public static class StoreContextSeed
	{
		public async static Task SeedAsync(StoreDbContext _dbContext)
		{

			if (!_dbContext.ProductBrands.Any())
			{
				var brandData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/brands.json");
				var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandData);


				if (brands?.Count() > 0)
				{
					foreach (var brand in brands)
					{
						_dbContext.Set<ProductBrand>().Add(brand);
					}
					await _dbContext.SaveChangesAsync();
				}
			}
			if (!_dbContext.ProductCategories.Any())
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
			if (!_dbContext.Products.Any())
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
			if (!_dbContext.DeliveryMethods.Any())
			{

				var delivaryMethodData = File.ReadAllText("../Talabat.Infrastructure/Data/DataSeed/delivery.json");
				var delivaryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(delivaryMethodData);


				if (delivaryMethods?.Count() > 0)
				{
					foreach (var deliveryMethod in delivaryMethods)
					{

						_dbContext.Set<DeliveryMethod>().Add(deliveryMethod);
					}
					await _dbContext.SaveChangesAsync();
				}
			}
		}
	}
}
