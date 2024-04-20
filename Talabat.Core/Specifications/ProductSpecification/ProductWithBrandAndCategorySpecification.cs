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

		public ProductWithBrandAndCategorySpecification(string sort)
			:base()
		{
			ApplyIncludes();

			if (!string.IsNullOrEmpty(sort))
			{
				switch (sort)
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
