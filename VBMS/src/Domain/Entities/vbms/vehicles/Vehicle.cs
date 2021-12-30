using System.ComponentModel.DataAnnotations.Schema;
using VBMS.Domain.Contracts;

namespace VBMS.Domain.Entities.vbms.vehicles
{
    public class Vehicle : AuditableEntity<int>
    {
        public string Rego { get; set; }
        public string Description { get; set; }
        [Column(TypeName = "text")]
        public string ImageDataURL { get; set; }
        public int VehicleTypeId { get; set; }
        public virtual VehicleType VehicleType { get; set; }
    }
}
