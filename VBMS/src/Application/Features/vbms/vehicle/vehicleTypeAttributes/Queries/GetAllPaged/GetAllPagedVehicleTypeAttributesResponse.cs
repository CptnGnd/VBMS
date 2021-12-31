using VBMS.Domain.Enums;

namespace VBMS.Application.Features.VehicleTypeAttributes.Queries.GetAllPaged
{
    public class GetAllPagedVehicleTypeAttributesResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AttributeType AttributeType { get; set; }
        public string VehicleType { get; set; }
        public int VehicleTypeId { get; set; }
    }
}
