using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Route.Talabat.APIs.Errors;
using Stripe;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Services.Contract;

namespace Route.Talabat.APIs.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<PaymentController> _logger;

        // This is your Stripe CLI webhook secret for testing your endpoint locally.
        private const string whSecret = "whsec_f8c2fc070bd732b1ae6f1d8fc953f16e9b006207be07b3d5165c5a22b70b3690";


        public PaymentController(
            IPaymentService paymentService,
            ILogger<PaymentController> logger)
        {
            _paymentService = paymentService;
            _logger = logger;
        }

        [Authorize(AuthenticationSchemes = "Bearer")]
        [ProducesResponseType(typeof(CustomerBasket) , StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse) , StatusCodes.Status400BadRequest)]
        [HttpPost("{basketid}")] // Get : /api/Payment/{basketid}
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);
            if (basket is null) return BadRequest(new ApiResponse(400 , "Somthing Wrong With basket!!"));

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> WebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
           
                var stripeEvent = EventUtility.ConstructEvent(json,
                    Request.Headers["Stripe-Signature"], whSecret);

                var paymentIntent = (PaymentIntent)stripeEvent.Data.Object;

            Order? order;

                // Handle the event
                switch (stripeEvent.Type)
                {
                    case Events.PaymentIntentSucceeded:
                        order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, true);
                        _logger.LogInformation("Order Is Succeeded {0}", order?.PaymentIntentId);
                        _logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);
                        break;
                    case Events.PaymentIntentPaymentFailed:
                        order = await _paymentService.UpdateOrderStatus(paymentIntent.Id, false);
                        _logger.LogInformation("Order Is Failed {0}", order?.PaymentIntentId);
                        _logger.LogInformation("Unhandled event type: {0}", stripeEvent.Type);
                        break;
                }

                return Ok();
            
        }
    }
}
