using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talabat.Core.Entities.Order_Aggregate
{
	public class OrderItem : BaseEntity
	{
		private OrderItem()
		{
		}

		public OrderItem(ProductItemOrder product, decimal price, int quntity)
		{
			Product = product;
			Price = price;
			Quntity = quntity;
		}

		public ProductItemOrder Product { get; set; } = null!;

        public decimal Price { get; set; }

        public int Quntity { get; set; }
    }
}
