using Core.Entities;
using Core.Interface;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly StoreContext _dbContext;

        public GenericRepository(StoreContext dbContext) => _dbContext = dbContext;

        public async Task<IReadOnlyList<T>> GetAllAsync()
            => await _dbContext.Set<T>().ToListAsync();

        public async Task<T> GetByIdAsync(int id)
            => await _dbContext.Set<T>().FindAsync(id);

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecification<T> spec)
            => await ApplySpecification(spec).ToListAsync();

        public async Task<T> GetEntityWithSpecAsync(ISpecification<T> spec)
            => await ApplySpecification(spec).FirstOrDefaultAsync();

        public async Task<int> CountAsync(ISpecification<T> spec)
            => await ApplySpecification(spec).CountAsync();

        public IQueryable<T> ApplySpecification(ISpecification<T> spec)
            => SpecificationEvaluator<T>.GetQuery(_dbContext.Set<T>().AsQueryable(), spec);

        public void Add(T entity) => _dbContext.Set<T>().Add(entity);
        
        public void Update(T entity)
        {
            _dbContext.Set<T>().Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }
        
        public void Delete(T entity)
            => _dbContext.Set<T>().Remove(entity);
    }
}
