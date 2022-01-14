using System;

namespace VBMS.Application.Features.vbms.diary.diary.Queries.GetAllPaged
{
    public class GetAllPagedDiarysResponse
    {
        public int Id { get; set; }
        public DateTime StartDatTime { get; set; }
        public DateTime EndDatTime { get; set; }
        public int VehicleId { get; set; }
        public string Vehicle { get; set; }
        public int DiaryTypeId { get; set; }
        public string DiaryType { get; set; }
        public int? BookingId { get; set; }
        public string Booking { get; set; }
    }
}
