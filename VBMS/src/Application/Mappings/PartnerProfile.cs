using AutoMapper;
using VBMS.Application.Features.vbms.partner.partner.Commands.AddEdit;
using VBMS.Domain.Entities.vbms.partners;

namespace VBMS.Application.Mappings
{
    public class PartnerProfile : Profile
    {
        public PartnerProfile()
        {
            CreateMap<AddEditPartnerCommand, Partner>().ReverseMap();
        }
    }
}