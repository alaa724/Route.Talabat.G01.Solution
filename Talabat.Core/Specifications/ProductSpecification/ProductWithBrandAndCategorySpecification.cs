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

		public ProductWithBrandAndCategorySpecification()
			:base()
		{
			ApplyIncludes();
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
