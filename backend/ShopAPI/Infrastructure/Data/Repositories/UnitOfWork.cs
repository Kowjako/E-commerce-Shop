using Core.Entities;
using Core.Interface;
using System.Collections;

namespace Infrastructure.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreContext _context;
        private Hashtable _repos;

        public UnitOfWork(StoreContext context)
        {
            _context = context;
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseEntity
        {
            if(_repos == null) _repos = new Hashtable();

            var type = typeof(TEntity).Name;
            if(!_repos.ContainsKey(type))
            {
                var repoType = typeof(GenericRepository<>);
                var repoInstance = Activator.CreateInstance(repoType.MakeGenericType(typeof(TEntity)), _context);
                _repos.Add(type, repoInstance);
            }

            return (IGenericRepository<TEntity>)_repos[type];
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
