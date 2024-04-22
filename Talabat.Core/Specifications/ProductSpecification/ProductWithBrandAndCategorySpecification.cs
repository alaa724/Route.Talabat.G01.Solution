using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;

namespace Talabat.Core.Specifications.ProductSpecification
{
	public class ProductWithBrandAndCategorySpecification : BaseSpecifications<Product>
	{

		public ProductWithBrandAndCategorySpecification(ProductSpecParams specParams)
			:base(P =>
					    (string.IsNullOrEmpty(specParams.Search) || P.Name.ToLower().Contains(specParams.Search)) &&
						(!specParams.BrandId.HasValue    || P.BrandId == specParams.BrandId.Value)&&
						(!specParams.CategoryId.HasValue || P.CategoryId == specParams.CategoryId.Value)
			
			)
		{
			ApplyIncludes();

			if (!string.IsNullOrEmpty(specParams.Sort))
			{
				switch (specParams.Sort)
				{

					case "priceAsc":
						AddOrderBy(P => P.Price);
						break;

					case "priceDesc":
						AddOrderByDesc(P => P.Price);
						break;

					default:
						AddOrderBy(P => P.Name);
						break;
				}
			}
			else
				AddOrderBy(P => P.Name);

			// TotalProduct = 18 , 20
			// pageSize = 5
			// pageIndex = 3

			ApplyPagination((specParams.PageIndex - 1) * specParams.PageSize, specParams.PageSize);

		}
			


		public ProductWithBrandAndCategorySpecification(int id)
			: base(P => P.Id == id)
		{
			ApplyIncludes();
		}

		private void ApplyIncludes()
		{
			Includes.Add(P => P.Brand);

			Includes.Add(P => P.Category);
		}

		
	}
}
