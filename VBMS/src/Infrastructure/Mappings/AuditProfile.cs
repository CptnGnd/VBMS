using AutoMapper;
using VBMS.Infrastructure.Models.Audit;
using VBMS.Application.Responses.Audit;

namespace VBMS.Infrastructure.Mappings
{
    public class AuditProfile : Profile
    {
        public AuditProfile()
        {
            CreateMap<AuditResponse, Audit>().ReverseMap();
        }
    }
}