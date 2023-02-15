using Core.Entities.OrderAggregate;

namespace Core.Specifications.Concrete
{
    public class OrdersWithItemsAndDeliverySpec : BaseSpecification<Order>
    {
        public OrdersWithItemsAndDeliverySpec(string email)
            : base(x => x.BuyerEmail== email)
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
            AddOrderByDesc(o => o.OrderDate);
        }

        public OrdersWithItemsAndDeliverySpec(int orderId, string buyerEmail)
            : base(x => x.Id == orderId && x.BuyerEmail == buyerEmail) 
        {
            AddInclude(o => o.OrderItems);
            AddInclude(o => o.DeliveryMethod);
        }
    }
}
