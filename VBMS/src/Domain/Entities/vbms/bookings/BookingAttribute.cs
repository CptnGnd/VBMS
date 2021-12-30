using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VBMS.Domain.Contracts;
using VBMS.Domain.Entities.vbms.vehicles;
using VBMS.Domain.Enums;

namespace VBMS.Domain.Entities.vbms.bookings
{
    public class BookingAttribute : AuditableEntity<int>
    {
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }
        public int VehicleAttributeId { get; set; }
        public virtual VehicleTypeAttribute VehicleTypeAttribute { get; set; }
        public string AttributeValue { get; set; }
        public AttributeComparer Comparer { get; set; }
    }
}
