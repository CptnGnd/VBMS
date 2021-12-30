using AutoMapper;
using VBMS.Application.Requests.Identity;
using VBMS.Application.Responses.Identity;

namespace VBMS.Client.Infrastructure.Mappings
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<PermissionResponse, PermissionRequest>().ReverseMap();
            CreateMap<RoleClaimResponse, RoleClaimRequest>().ReverseMap();
        }
    }
}