using Microsoft.Extensions.Configuration;
using Stripe;
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
using Talabat.Core.Specifications.PaymentSpecification;
using Product = Talabat.Core.Entities.Product;

namespace Talabat.Application.PaymentServices
{
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepo;
        private readonly IUniteOfWork _uniteOfWork;

        public PaymentService(
            IConfiguration configuration,
            IBasketRepository basketRepo,
            IUniteOfWork uniteOfWork)
        {
            _configuration = configuration;
            _basketRepo = basketRepo;
            _uniteOfWork = uniteOfWork;
        }

        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettinges:Secretkey"];

            var basket = await _basketRepo.GetBasketAsync(basketId);
            if (basket is null) return null;
                
            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _uniteOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Cost;
                basket.ShippingPrice = shippingPrice;
            }

            if(basket.Items.Count > 0)
            {
                var productRepo = _uniteOfWork.Repository<Product>();

                foreach (var item in basket.Items)
                {
                    var product = await productRepo.GetAsync(item.Id);

                    if (item.Price != product.Price)
                        item.Price = product.Price;

                }
            }

            PaymentIntent paymentIntent;
            PaymentIntentService paymentIntentService = new PaymentIntentService();

            if (string.IsNullOrEmpty(basket.PaymentIntentId)) // Create New PaymentIntent
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quntity) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(options); // Integration With Stripe

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else // Update Existing PaymentIntent
            {
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quntity) + (long)shippingPrice * 100
                };

                /*paymentIntent = */await paymentIntentService.UpdateAsync(basket.PaymentIntentId,options);
            }

            await _basketRepo.UpdateBasketAsync(basket);

            return basket;
        }

        public async Task<Order?> UpdateOrderStatus(string paymentIntentId, bool isPaied)
        {
            var orderRepo = _uniteOfWork.Repository<Order>();
            var spec = new OrderWithPaymentIntentspecifications(paymentIntentId);

            var order = await orderRepo.GetWithSpecAsync(spec);

            if (order is null) return null;

            if (isPaied)
                order.Status = OrderStatus.PaymentRecive;

            else
                order.Status = OrderStatus.PaymentFailed;

            orderRepo.Update(order);

            await _uniteOfWork.CompleteAsync();

            return order;
        }
    }
}
