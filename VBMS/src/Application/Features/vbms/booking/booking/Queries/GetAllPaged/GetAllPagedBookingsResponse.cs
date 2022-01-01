using System;

namespace VBMS.Application.Features.vbms.booking.booking.Queries.GetAllPaged
{
    public class GetAllPagedBookingsResponse
    {
        public int Id { get; set; }
        public string BookingCode { get; set; }
        public string BookingType { get; set; }
        public string Partner { get; set; }
        public int PartnerId { get; set; }
        public string VehicleType { get; set; }
        public int VehicleTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
