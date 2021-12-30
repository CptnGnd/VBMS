using VBMS.Application.Responses.Identity;
using VBMS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Chat;
using VBMS.Application.Models.Chat;

namespace VBMS.Application.Interfaces.Services
{
    public interface IChatService
    {
        Task<Result<IEnumerable<ChatUserResponse>>> GetChatUsersAsync(string userId);

        Task<IResult> SaveMessageAsync(ChatHistory<IChatUser> message);

        Task<Result<IEnumerable<ChatHistoryResponse>>> GetChatHistoryAsync(string userId, string contactId);
    }
}