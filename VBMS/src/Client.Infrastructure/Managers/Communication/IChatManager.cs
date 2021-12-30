using VBMS.Application.Models.Chat;
using VBMS.Application.Responses.Identity;
using VBMS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Chat;

namespace VBMS.Client.Infrastructure.Managers.Communication
{
    public interface IChatManager : IManager
    {
        Task<IResult<IEnumerable<ChatUserResponse>>> GetChatUsersAsync();

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> chatHistory);

        Task<IResult<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string cId);
    }
}