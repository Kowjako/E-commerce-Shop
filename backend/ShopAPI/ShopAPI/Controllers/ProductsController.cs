using AutoMapper;
using Core.Entities;
using Core.Interface;
using Core.Specifications.Concrete;
using Core.Specifications.Params;
using Microsoft.AspNetCore.Mvc;
using ShopAPI.DTO;
using ShopAPI.Errors;
using ShopAPI.Helpers;

namespace ShopAPI.Controllers
{
    public class ProductsController : BaseApiController
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
        public async Task<ActionResult<Pagination<ProductDTO>>> GetProducts([FromQuery]ProductsSpecParams @params)
        {
            var products = await _repo.GetAllWithSpecAsync(new ProductsWithTypesAndBrandsSpec(@params));
            var count = await _repo.CountAsync(new ProductsWithFiltersForCountSpec(@params));

            var data = _mapper.Map<IReadOnlyList<ProductDTO>>(products);

            var paginationResult = new Pagination<ProductDTO>
            {
                Count = count,
                Data = data,
                PageSize = @params.PageSize,
                PageIndex = @params.PageIndex
            };

            return Ok(paginationResult);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductDTO))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ApiResponse))]
        public async Task<ActionResult<ProductDTO>> GetProduct([FromRoute] int id)
        {
            var product = await _repo.GetEntityWithSpecAsync(new ProductsWithTypesAndBrandsSpec(id));

            if (product == null)
            {
                return NotFound(new ApiResponse(404)); //pass object to body
            }

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
