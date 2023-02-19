using Core.Entities.OrderAggregate;
using System.Linq.Expressions;

namespace Core.Specifications.Concrete
{
    public class OrderByPaymentIntentIdSpec : BaseSpecification<Order>
    {
        public OrderByPaymentIntentIdSpec(string paymentIntentId) 
            : base(x => x.PaymentIntentId.Equals(paymentIntentId))
        {
        }
    }
}
