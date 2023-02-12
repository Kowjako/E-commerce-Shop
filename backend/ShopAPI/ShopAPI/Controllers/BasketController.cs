using AutoMapper;
using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO;

namespace ShopAPI.Controllers
{
    //docker-compose up --detach
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _repo;
        private readonly IMapper _mapper;

        public BasketController(IBasketRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById([FromQuery]string id)
        {
            var basket = await _repo.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket([FromBody]CustomerBasketDTO basket)
        {
            var basketEntry = _mapper.Map<CustomerBasket>(basket);

            var updatedBasket = await _repo.UpdateBasketAsync(basketEntry);
            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteBasket([FromQuery]string id)
        {
            _ = await _repo.DeleteBasketAsync(id);
            return NoContent();
        }
    }
}
