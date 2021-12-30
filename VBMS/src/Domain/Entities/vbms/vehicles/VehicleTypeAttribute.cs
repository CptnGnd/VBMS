using VBMS.Domain.Contracts;
using VBMS.Domain.Enums;

namespace VBMS.Domain.Entities.vbms.vehicles
{
    public class VehicleTypeAttribute : AuditableEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public AttributeType AttributeType { get; set; }
        public int VehicleTypeId { get; set; }
        public virtual VehicleType VehicleType { get; set; }
    }
}
