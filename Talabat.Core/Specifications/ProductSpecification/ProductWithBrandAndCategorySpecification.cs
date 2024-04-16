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
			Includes.Add(P => P.Brand);

			Includes.Add(P => P.Category);
		}

	}
}
