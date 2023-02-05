using Core.Entities;
using Core.Interface;
using Core.Specifications.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace ShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _repo;
        private readonly IGenericRepository<ProductBrand> _prodBrandRepo;
        private readonly IGenericRepository<ProductType> _prodTypeRepo;

        public ProductsController(IGenericRepository<Product> repo, IGenericRepository<ProductBrand> prodBrandRepo,
            IGenericRepository<ProductType> prodTypeRepo)
        {
            _repo = repo;
            _prodBrandRepo = prodBrandRepo;
            _prodTypeRepo = prodTypeRepo;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await _repo.GetAllWithSpecAsync(new ProductsWithTypesAndBrandsSpec());
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct([FromRoute]int id)
        {
            var product = await _repo.GetEntityWithSpecAsync(new ProductsWithTypesAndBrandsSpec(id));
            return Ok(product);
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _prodBrandRepo.GetAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductTypes()
        {
            return Ok(await _prodTypeRepo.GetAllAsync());
        }
    }
}
