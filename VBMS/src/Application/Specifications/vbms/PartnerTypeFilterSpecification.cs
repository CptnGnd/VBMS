using VBMS.Application.Specifications.Base;
using VBMS.Domain.Entities.vbms.partners;

namespace VBMS.Application.Specifications.vbms
{
    public class PartnerTypeFilterSpecification : HeroSpecification<PartnerType>
    {
        public PartnerTypeFilterSpecification(string searchString)
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
