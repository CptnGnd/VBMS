using VBMS.Application.Specifications.Base;
using VBMS.Domain.Entities.Catalog;

namespace VBMS.Application.Specifications.Catalog
{
    public class BrandTestFilterSpecification : HeroSpecification<BrandTest>
    {
        public BrandTestFilterSpecification(string searchString)
        {
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name.Contains(searchString) || p.Description.Contains(searchString);
            }
            else
            {
                Criteria = p => true;
            }
        }
    }
}
