using AutoMapper;
using VBMS.Application.Features.vbms.vehicle.vehicle.Commands.AddEdit;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Mappings
{
    public class VehicleProfile : Profile
    {
        public VehicleProfile()
        {
            CreateMap<AddEditVehicleCommand, Vehicle>().ReverseMap();
        }
    }
}