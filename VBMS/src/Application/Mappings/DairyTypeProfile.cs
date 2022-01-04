using AutoMapper;
using VBMS.Application.Features.vbms.diary.dairyType.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.dairyType.Queries.GetAll;
using VBMS.Application.Features.vbms.diary.dairyType.Queries.GetById;
using VBMS.Domain.Entities.vbms.diary;

namespace VBMS.Application.Mappings
{
    public class DairyTypeProfile : Profile
    {
        public DairyTypeProfile()
        {
            CreateMap<AddEditDairyTypeCommand, DairyType>().ReverseMap();
            CreateMap<GetDairyTypeByIdResponse, DairyType>().ReverseMap();
            CreateMap<GetAllDairyTypesResponse, DairyType>().ReverseMap();
        }
    }
}