using AutoMapper;
using VBMS.Application.Features.vbms.partner.partnerType.Commands.AddEdit;
using VBMS.Application.Features.vbms.partner.partnerType.Queries.GetAll;
using VBMS.Application.Features.vbms.partner.partnerType.Queries.GetById;
using VBMS.Domain.Entities.vbms.partners;

namespace VBMS.Application.Mappings
{
    public class PartnerTypeProfile : Profile
    {
        public PartnerTypeProfile()
        {
            CreateMap<AddEditPartnerTypeCommand, PartnerType>().ReverseMap();
            CreateMap<GetPartnerTypeByIdResponse, PartnerType>().ReverseMap();
            CreateMap<GetAllPartnerTypesResponse, PartnerType>().ReverseMap();
        }
    }
}