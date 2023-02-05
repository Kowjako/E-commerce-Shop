using Core.Entities;
using Core.Interface;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _dbContext;

        public ProductRepository(StoreContext dbContext) => _dbContext = dbContext;
        
        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
            => await _dbContext.ProductBrand.ToListAsync();
        
        public async Task<Product> GetProductByIdAsync(int id)
            => await _dbContext.Products.Include(p => p.ProductBrand)
                                        .Include(p => p.ProductType)
                                        .FirstOrDefaultAsync(p => p.Id == id);
        
        public async Task<IReadOnlyList<Product>> GetProductsAsync()
            => await _dbContext.Products.Include(p => p.ProductBrand)
                                        .Include(p => p.ProductType)
                                        .ToListAsync();
        
        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
            => await _dbContext.ProductTypes.ToListAsync();
    }
}
