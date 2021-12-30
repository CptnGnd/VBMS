using AutoMapper;
using VBMS.Infrastructure.Models.Identity;
using VBMS.Application.Responses.Identity;

namespace VBMS.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleResponse, BlazorHeroRole>().ReverseMap();
        }
    }
}