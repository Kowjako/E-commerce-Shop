using Core.Entities;

namespace Core.Interface
{
    public interface IPaymentService
    {
        Task<CustomerBasket> CreateOrUpdatePaymentIntent(string basketId);
    }
}
