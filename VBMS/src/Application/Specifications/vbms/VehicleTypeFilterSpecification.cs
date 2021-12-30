using VBMS.Application.Specifications.Base;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Specifications.Catalog
{
    public class VehicleTypeFilterSpecification : HeroSpecification<VehicleType>
    {
        public VehicleTypeFilterSpecification(string searchString)
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
