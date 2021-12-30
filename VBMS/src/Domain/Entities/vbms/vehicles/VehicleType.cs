using VBMS.Domain.Contracts;

namespace VBMS.Domain.Entities.vbms.vehicles
{
    public class VehicleType : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
