namespace VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetAllPaged
{
    public class GetAllPagedVehiclesResponse
    {
        public int Id { get; set; }
        public string Rego { get; set; }
        public string Description { get; set; }
        public string VehicleType { get; set; }
        public int VehicleTypeId { get; set; }
    }
}
