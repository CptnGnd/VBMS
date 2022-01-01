using System;
using VBMS.Domain.Contracts;
using VBMS.Domain.Entities.vbms.partners;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Domain.Entities.vbms.bookings
{
    public class Booking : AuditableEntity<int>
    {
        public string BookingCode { get; set; }
        public string BookingType { get; set;}
        public int PartnerId { get; set; }
        public virtual Partner Partner { get; set; }
        public int VehicleTypeId { get; set; }
        public virtual VehicleType VehicleType { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Now;
        public DateTime EndDate { get; set; }   
    }
}
