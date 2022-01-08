namespace VBMS.Application.Features.vbms.booking.booking.Queries.GetAll;

public class GetAllBookingsResponse
{
    public int Id { get; set; }
    public string BookingCode { get; set; }
    public string BookingType { get; set; }
    public int PartnerId { get; set; }
    public string Partner { get; set; }
    public int VehicleTypeId { get; set; }
    public string VehicleType { get; set; }
}
