using Core.Entities;
using Core.Specifications.Params;

namespace Core.Specifications.Concrete
{
    public class ProductsWithFiltersForCountSpec : BaseSpecification<Product>
    {
        public ProductsWithFiltersForCountSpec(ProductsSpecParams @params)
            : base(x =>
                (!string.IsNullOrEmpty(@params.Search) || x.Name.ToLower().Contains(@params.Search)) &&
                (!@params.BrandId.HasValue || x.ProductBrandId == @params.BrandId) &&
                (!@params.TypeId.HasValue || x.ProductTypeId == @params.TypeId)
            )
        { }
    }
}
