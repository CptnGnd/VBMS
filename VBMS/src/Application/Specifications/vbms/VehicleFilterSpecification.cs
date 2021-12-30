using VBMS.Application.Specifications.Base;
using VBMS.Domain.Entities.Catalog;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Specifications.vbms
{
    public class VehicleFilterSpecification : HeroSpecification<Vehicle>
    {
        public VehicleFilterSpecification(string searchString)
        {
            Includes.Add(a => a.VehicleType);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.Rego != null && (p.Rego.Contains(searchString) || p.Description.Contains(searchString) || p.VehicleType.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.Rego != null;
            }
        }
    }
}