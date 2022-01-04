using AutoMapper;
using VBMS.Application.Features.vbms.diary.diaryType.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.diaryType.Queries.GetAll;
using VBMS.Application.Features.vbms.diary.diaryType.Queries.GetById;
using VBMS.Domain.Entities.vbms.diary;

namespace VBMS.Application.Mappings
{
    public class DiaryTypeProfile : Profile
    {
        public DiaryTypeProfile()
        {
            CreateMap<AddEditDiaryTypeCommand, DiaryType>().ReverseMap();
            CreateMap<GetDiaryTypeByIdResponse, DiaryType>().ReverseMap();
            CreateMap<GetAllDiaryTypesResponse, DiaryType>().ReverseMap();
        }
    }
}