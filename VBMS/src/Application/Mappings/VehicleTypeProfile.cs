using AutoMapper;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Commands.AddEdit;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.GetAll;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.GetById;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Mappings
{
    public class VehicleTypeProfile : Profile
    {
        public VehicleTypeProfile()
        {
            CreateMap<AddEditVehicleTypeCommand, VehicleType>().ReverseMap();
            CreateMap<GetVehicleTypeByIdResponse, VehicleType>().ReverseMap();
            CreateMap<GetAllVehicleTypesResponse, VehicleType>().ReverseMap();
        }
    }
}