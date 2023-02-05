using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications.Concrete;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO;

namespace ShopAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IGenericRepository<Product> _repo;
        private readonly IGenericRepository<ProductBrand> _prodBrandRepo;
        private readonly IGenericRepository<ProductType> _prodTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> repo, IGenericRepository<ProductBrand> prodBrandRepo,
            IGenericRepository<ProductType> prodTypeRepo, IMapper mapper)
        {
            _repo = repo;
            _prodBrandRepo = prodBrandRepo;
            _prodTypeRepo = prodTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDTO>>> GetProducts()
        {
            var products = await _repo.GetAllWithSpecAsync(new ProductsWithTypesAndBrandsSpec());
            return Ok(_mapper.Map<IReadOnlyList<ProductDTO>>(products));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDTO>> GetProduct([FromRoute]int id)
        {
            var product = await _repo.GetEntityWithSpecAsync(new ProductsWithTypesAndBrandsSpec(id));
            return Ok(_mapper.Map<ProductDTO>(product));
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
