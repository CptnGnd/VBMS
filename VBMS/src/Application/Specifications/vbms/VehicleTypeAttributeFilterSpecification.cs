using VBMS.Application.Specifications.Base;
using VBMS.Domain.Entities.Catalog;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Specifications.vbms
{
    public class VehicleTypeAttributeFilterSpecification : HeroSpecification<VehicleTypeAttribute>
    {
        public VehicleTypeAttributeFilterSpecification(string searchString)
        {
            Includes.Add(a => a.VehicleType);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Name != null && (p.Name.Contains(searchString) || p.Description.Contains(searchString)  || p.VehicleType.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.Name != null;
            }
        }
    }
}