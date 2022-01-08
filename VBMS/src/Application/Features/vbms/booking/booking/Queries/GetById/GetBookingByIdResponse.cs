using System;

namespace VBMS.Application.Features.vbms.booking.booking.Queries.GetById
{
    public class GetBookingByIdResponse
    {
        public int VehicleId { get; set; }
       // public virtual Vehicle Vehicle { get; set; }
        public int BookingId { get; set; } = 0;
      //  public virtual Booking Booking { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int DiaryTypeId { get; set; }
      //  public virtual DiaryType DiaryType { get; set; }
    }
}

