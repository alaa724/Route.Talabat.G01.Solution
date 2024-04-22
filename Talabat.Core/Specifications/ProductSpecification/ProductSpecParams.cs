using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Specifications.ProductSpecification
{
	public class ProductSpecParams
	{
		int maxPageSize = 10;
		private int pageSize = 5;

		public int PageSize
		{
			get { return pageSize; }
			set { pageSize = value > pageSize ? pageSize : value; }
		}

		public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; }
        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }
    }
}
