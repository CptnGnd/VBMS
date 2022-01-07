using AutoMapper;
using VBMS.Application.Features.vbms.diary.diary.Commands.AddEdit;
using VBMS.Domain.Entities.vbms.diary;

namespace VBMS.Application.Mappings
{
    public class DiaryProfile : Profile
    {
        public DiaryProfile()
        {
            CreateMap<AddEditDiaryCommand, Diary>().ReverseMap();
        }
    }
}