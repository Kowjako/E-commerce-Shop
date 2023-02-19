using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.Errors;

namespace ShopAPI.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentSvc;

        public PaymentController(IPaymentService paymentSvc)
        {
            _paymentSvc = paymentSvc;
        }

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePaymentIntent(string basketId)
        {
            var basket = await _paymentSvc.CreateOrUpdatePaymentIntent(basketId);
            if(basket == null)
            {
                return BadRequest(new ApiResponse(400, "Problem with your basket"));
            }
            return Ok(basket);
        }
    } 
}
