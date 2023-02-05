using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> entityQuery, ISpecification<TEntity> spec)
        {
            var query = entityQuery;

            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            return spec.Includes.Aggregate(query, (current, include) => current.Include(include));
        }
    }
}
