using System;
using VBMS.Domain.Contracts;
using VBMS.Domain.Entities.vbms.bookings;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Domain.Entities.vbms.diary
{
    public class Dairy : AuditableEntity<int>
    {
        public int VehicleId { get; set; }
        public virtual Vehicle Vehicle { get; set;}
        public int BookingId { get; set; } = 0;
        public virtual Booking Booking { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int DiaryTypeId { get; set; }
        public virtual DairyType DairyType { get; set; }
    }
}
