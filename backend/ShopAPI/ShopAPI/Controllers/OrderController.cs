using AutoMapper;
using Core.Entities.OrderAggregate;
using Core.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO;
using ShopAPI.Errors;
using System.Security.Claims;

namespace ShopAPI.Controllers
{
    [Authorize]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public OrderController(IOrderService orderService, IMapper mapper) 
        {
            _orderService = orderService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Order>> CreateOrder([FromBody]OrderDTO order)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = _mapper.Map<Address>(order.ShipToAddress);

            var orderToSave = await _orderService.CreateOrderAsync(email, order.DeliveryId, order.BasketId, address);
            if(orderToSave != null)
            {
                return Ok(orderToSave);
            }
            return BadRequest(new ApiResponse(400, "Problem creating order"));
        }
    }
}
