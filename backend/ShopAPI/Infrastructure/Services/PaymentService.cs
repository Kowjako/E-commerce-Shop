using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interface;
using Microsoft.Extensions.Configuration;
using Stripe;

namespace Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public PaymentService(IBasketRepository basketRepo, IUnitOfWork uow, IConfiguration config)
        {
            _basketRepo = basketRepo;
            _uow = uow;
            _config = config;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
            var basket = await _basketRepo.GetBasketAsync(basketId);

            if (basket == null)
            {
                return null;
            }

            var shippingPrice = 0m;
            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _uow.Repository<DeliveryMethod>().GetByIdAsync(basket.DeliveryMethodId.Value);
                shippingPrice = deliveryMethod.Price;
            }

            foreach (var item in basket.Items)
            {
                var productItem = await _uow.Repository<Core.Entities.Product>().GetByIdAsync(item.Id);

                // we cant trust the client
                if (item.Price != productItem.Price) item.Price = productItem.Price;
            }

            var service = new PaymentIntentService();
            PaymentIntent intent;

            // If we're creating new payment intent
            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(p => p.Quantity * (p.Price * 100)) + (long)shippingPrice * 100,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                intent = await service.CreateAsync(options);
                // Store paymentIntent and ClientSecret inside basket, if the user
                // will decide to change basket content
                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                // We are updating existing payment intent
                var options = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(p => p.Quantity * (p.Price * 100)) + (long)shippingPrice * 100
                };
                await service.UpdateAsync(basket.PaymentIntentId, options);
            }

            await _basketRepo.UpdateBasketAsync(basket);
            return basket;
        }
    }
}
