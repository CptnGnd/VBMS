namespace VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetAll;

public class GetAllVehiclesResponse
{
    public int Id { get; set; }
    public string Rego { get; set; }
    public string Description { get; set; }
    public string ImageDataURL { get; set; }
    public int VehicleTypeId { get; set; }
    public string VehicleType { get; set; }
}
