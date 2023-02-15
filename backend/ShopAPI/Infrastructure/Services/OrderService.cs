using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interface;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IUnitOfWork _uow;

        public OrderService(IBasketRepository basketRepo, IUnitOfWork uow)
        {
            _basketRepo = basketRepo;
            _uow = uow;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            // 1. Get basket from Redis
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2. Get items themselves to check price and create order items
            var items = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var productItem = await _uow.Repository<Product>().GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            // 3. Get delivery Method
            var delMethod = await _uow.Repository<DeliveryMethod>().GetByIdAsync(deliveryMethodId);

            // 4. Calculate subtotal based on price retrieved from repo
            var subTotal = items.Aggregate(0m, (sum, p) => sum + p.Quantity * p.Price);

            // 5. Create order
            var order = new Order(items, buyerEmail, shippingAddress, delMethod, subTotal);
            _uow.Repository<Order>().Add(order);

            // 6. Save to db
            var result = await _uow.Complete();
            if (result <= 0) return null; 

            // 7. Delete basket
            await _basketRepo.DeleteBasketAsync(basketId);

            // 8. Return brand new order
            return order;
        }

        public Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetOrderByIdAsync(int id, string buyerEmail)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}
