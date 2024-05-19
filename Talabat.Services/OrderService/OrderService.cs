using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repository.Contract;
using Talabat.Core.Services.Contract;
using Talabat.Core.Specifications.OrderSpecification;

namespace Talabat.Application.OrderService
{
	public class OrderService : IOrderService
	{
		private readonly IBasketRepository _basketRepo;
		private readonly IUniteOfWork _uniteOfWork;

		///private readonly IGenericRepository<Product> _productRepo;
		///private readonly IGenericRepository<DeliveryMethod> _deliveryMethodRepo;
		///private readonly IGenericRepository<Order> _orderRepo;

		public OrderService(
			IBasketRepository basketRepo,
			IUniteOfWork uniteOfWork
			///IGenericRepository<Product> productRepo,
			///IGenericRepository<DeliveryMethod> deliveryMethodRepo,
			///IGenericRepository<Order> orderRepo
			)
		{
			_basketRepo = basketRepo;
			_uniteOfWork = uniteOfWork;
			///_productRepo = productRepo;
			///_deliveryMethodRepo = deliveryMethodRepo;
			///_orderRepo = orderRepo;
		}

		public async Task<Order?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, Address shippingAddress)
		{
			// 1.Get Basket From Baskets Repo

			var basket = await _basketRepo.GetBasketAsync(basketId);

			// 2. Get Selected Items at Basket From Products Repo

			var orderItems = new List<OrderItem>();
			
			if(basket?.Items?.Count > 0)
			{
				foreach(var item in basket.Items)
				{
					var product = await _uniteOfWork.Repository<Product>().GetAsync(item.Id);

					var productItemOrdered = new ProductItemOrder(product.Id, product.Name, product.PictureUrl);

					var orderItem = new OrderItem(productItemOrdered, product.Price, item.Quntity);

					orderItems.Add(orderItem);
				}
			}

			// 3. Calculate SubTotal

			var subTotal = orderItems.Sum(item => item.Quntity * item.Price);

			// 4. Get Delivery Method From DeliveryMethods Repo

			//var deliveryMethod = await _deliveryMethodRepo.GetAsync(deliveryMethodId);

			// 5. Create Order

			var order = new Order(
				buyerEmail: buyerEmail,
				shippingAddress: shippingAddress,
				deliveryMethodId: deliveryMethodId,
				items: orderItems,
				subtotal: subTotal
				);

			_uniteOfWork.Repository<Order>().Add(order);

			// 6. Save To Database [TODO]

			var result = await _uniteOfWork.CompleteAsync();

			if (result <= 0) return null;

			return order;

		}

		public async Task<IReadOnlyList<Order>> GetOrderForUserAsync(string buyerEmail)
		{
			var ordersRepo = _uniteOfWork.Repository<Order>();

			var spec = new OrderSpecifications(buyerEmail);

			var orders = await ordersRepo.GetAllWithSpecAsync(spec);

			return orders;
		}

		public Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
		{
			var orderRepo = _uniteOfWork.Repository<Order>();

			var spec = new OrderSpecifications(orderId, buyerEmail);

			var order = orderRepo.GetWithSpecAsync(spec);

			return order;
		}


		public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
			=> await _uniteOfWork.Repository<DeliveryMethod>().GetAllAsync();

	}
}
