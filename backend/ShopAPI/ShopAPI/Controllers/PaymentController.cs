using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
            return await _paymentSvc.CreateOrUpdatePaymentIntent(basketId);
        }
    } 
}
