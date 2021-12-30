using VBMS.Application.Specifications.Base;
using VBMS.Domain.Entities.Catalog;

namespace VBMS.Application.Specifications.Catalog
{
    public class ProductTestFilterSpecification : HeroSpecification<ProductTest>
    {
        public ProductTestFilterSpecification(string searchString)
        {
            Includes.Add(a => a.BrandTest);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Barcode != null && (p.Name.Contains(searchString) || p.Description.Contains(searchString) || p.Barcode.Contains(searchString) || p.BrandTest.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.Barcode != null;
            }
        }
    }
}