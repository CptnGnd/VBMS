using AutoMapper;
using VBMS.Application.Interfaces.Chat;
using VBMS.Application.Models.Chat;
using VBMS.Infrastructure.Models.Identity;

namespace VBMS.Infrastructure.Mappings
{
    public class ChatHistoryProfile : Profile
    {
        public ChatHistoryProfile()
        {
            CreateMap<ChatHistory<IChatUser>, ChatHistory<BlazorHeroUser>>().ReverseMap();
        }
    }
}