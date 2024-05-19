
namespace Talabat.Core.Entities
{
	public class Product : BaseEntity
	{
		
		public string Name { get; set; }
        public string Description { get; set; }
        public string PictureUrl { get; set; }
        public decimal Price { get; set; }

        public int BrandId { get; set; } // FK => ProductBrand
        public /*virtual*/ ProductBrand Brand { get; set; } // Navigational Propert [One]

        public int CategoryId { get; set; } // FK => ProductCategory
        public /*virtual*/ ProductCategory Category { get; set; } // Navigational Propert [One]

	}
}
