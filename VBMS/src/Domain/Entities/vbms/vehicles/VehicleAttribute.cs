using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBMS.Domain.Contracts;

namespace VBMS.Domain.Entities.vbms.vehicles;

public class VehicleAttribute : AuditableEntity<int>
{
    public int VehicleId { get; set; }
    public virtual Vehicle Vehicle { get; set; }
    public int VehicleTypeAttributeId { get; set; }
    public virtual VehicleTypeAttribute VehicleTypeAttribute { get; set; }
    public DateTime ValidFrom { get; set; } = DateTime.Now;
    public DateTime ValidTo { get; set; } = DateTime.MaxValue;
    public string AttributeValue { get; set; }
}
