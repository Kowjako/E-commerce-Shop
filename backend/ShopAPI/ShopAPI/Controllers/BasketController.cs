using Core.Entities;
using Core.Interface;
using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Controllers
{
    //docker-compose up --detach
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _repo;

        public BasketController(IBasketRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasketById([FromQuery]string id)
        {
            var basket = await _repo.GetBasketAsync(id);
            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket([FromBody]CustomerBasket basket)
        {
            var updatedBasket = await _repo.UpdateBasketAsync(basket);
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
