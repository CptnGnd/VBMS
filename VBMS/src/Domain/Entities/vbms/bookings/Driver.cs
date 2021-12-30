using System;
using VBMS.Domain.Contracts;

namespace VBMS.Domain.Entities.vbms.bookings
{
    public class Driver : AuditableEntity<int>
    {
        public int BookingId { get; set; }
        public virtual Booking Booking { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string LisenceID { get; set; }
        public string Country { get; set; }
        public DateTime Expiry { get; set; }
        public string ImageDataURL { get; set; }
    }
}
