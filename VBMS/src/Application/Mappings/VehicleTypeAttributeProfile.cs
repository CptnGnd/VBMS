using AutoMapper;
using VBMS.Application.Features.VehicleTypeAttributes.Commands.AddEdit;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Mappings
{
    public class VehicleTypeAttributeProfile : Profile
    {
        public VehicleTypeAttributeProfile()
        {
            CreateMap<AddEditVehicleTypeAttributeCommand, VehicleTypeAttribute>().ReverseMap();
        }
    }
}