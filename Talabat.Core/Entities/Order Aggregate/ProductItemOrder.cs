﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class ProductItemOrder
	{
		// Must Use ParameterLess Constractor for EF Core
		private ProductItemOrder()
		{
		}

		public ProductItemOrder(int productId, string productName, string productUrl)
		{
			ProductId = productId;
			ProductName = productName;
			ProductUrl = productUrl;
		}

		public int ProductId { get; set; }
		public string ProductName { get; set; } = null!;
		public string ProductUrl { get; set; } = null!;
	}
}
