using VBMS.Application.Interfaces.Common;
using VBMS.Application.Requests.Identity;
using VBMS.Application.Responses.Identity;
using VBMS.Shared.Wrapper;
using System.Threading.Tasks;

namespace VBMS.Application.Interfaces.Services.Identity
{
    public interface ITokenService : IService
    {
        Task<Result<TokenResponse>> LoginAsync(TokenRequest model);

        Task<Result<TokenResponse>> GetRefreshTokenAsync(RefreshTokenRequest model);
    }
}