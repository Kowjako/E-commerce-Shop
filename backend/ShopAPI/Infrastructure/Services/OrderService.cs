using Core.Entities;
using Core.Entities.OrderAggregate;
using Core.Interface;

namespace Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepo;
        private readonly IGenericRepository<Order> _orderRepo;
        private readonly IGenericRepository<DeliveryMethod> _delMethodRepo;
        private readonly IGenericRepository<Product> _productRepo;

        public OrderService(IBasketRepository basketRepo, IGenericRepository<Order> orderRepo,
            IGenericRepository<DeliveryMethod> delMethodRepo, IGenericRepository<Product> productRepo)
        {
            _basketRepo = basketRepo;
            _orderRepo = orderRepo;
            _delMethodRepo = delMethodRepo;
            _productRepo  = productRepo;
        }

        public async Task<Order> CreateOrderAsync(string buyerEmail, int deliveryMethodId, string basketId, Address shippingAddress)
        {
            // 1. Get basket from Redis
            var basket = await _basketRepo.GetBasketAsync(basketId);

            // 2. Get items themselves to check price and create order items
            var items = new List<OrderItem>();
            foreach(var item in basket.Items)
            {
                var productItem = await _productRepo.GetByIdAsync(item.Id);
                var itemOrdered = new ProductItemOrdered(productItem.Id, productItem.Name, productItem.PictureUrl);
                var orderItem = new OrderItem(itemOrdered, productItem.Price, item.Quantity);
                items.Add(orderItem);
            }

            // 3. Get delivery Method
            var delMethod = await _delMethodRepo.GetByIdAsync(deliveryMethodId);

            // 4. Calculate subtotal based on price retrieved from repo
            var subTotal = items.Aggregate(0m, (sum, p) => sum + p.Quantity * p.Price);

            // 5. Create order
            var order = new Order(items, buyerEmail, shippingAddress, delMethod, subTotal);

            // 6. Save to db (TODO)

            // 7. Return brand new order
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
