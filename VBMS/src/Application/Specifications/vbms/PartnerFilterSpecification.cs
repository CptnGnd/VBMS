using VBMS.Application.Specifications.Base;
using VBMS.Domain.Entities.Catalog;
using VBMS.Domain.Entities.vbms.partners;

namespace VBMS.Application.Specifications.vbms
{
    public class PartnerFilterSpecification : HeroSpecification<Partner>
    {
        public PartnerFilterSpecification(string searchString)
        {
            Includes.Add(a => a.PartnerType);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name != null && (p.Name.Contains(searchString) || p.ShortName.Contains(searchString) || p.PartnerType.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.Name != null;
            }
        }
    }
}