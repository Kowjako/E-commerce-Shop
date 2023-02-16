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
        public async Task<ActionResult<OrderToReturnDTO>> CreateOrder([FromBody]OrderDTO order)
        {
            // since we have Authroize attr on controller we have acces to user
            var email = User.FindFirstValue(ClaimTypes.Email);
            var address = _mapper.Map<Address>(order.ShipToAddress);

            var orderToSave = await _orderService.CreateOrderAsync(email, order.DeliveryId, order.BasketId, address);
            if(orderToSave != null)
            {
                return Ok(_mapper.Map<OrderToReturnDTO>(orderToSave));
            }
            return BadRequest(new ApiResponse(400, "Problem creating order"));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDTO>>> GetOrdersForUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var orders = await _orderService.GetOrdersForUserAsync(email);
            return Ok(_mapper.Map<IReadOnlyList<OrderToReturnDTO>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDTO>> GetOrderById([FromRoute]int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var order = await _orderService.GetOrderByIdAsync(id, email);
            if (order == null)
                return NotFound(new ApiResponse(404));
            return Ok(_mapper.Map<OrderToReturnDTO>(order));
        }

        //[AllowAnonymous] allow to bypass Authorization attribute
        [HttpGet("delivery-methods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}
