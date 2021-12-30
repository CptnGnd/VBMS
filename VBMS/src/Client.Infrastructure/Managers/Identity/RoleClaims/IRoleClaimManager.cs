using System.Collections.Generic;
using System.Threading.Tasks;
using VBMS.Application.Requests.Identity;
using VBMS.Application.Responses.Identity;
using VBMS.Shared.Wrapper;

namespace VBMS.Client.Infrastructure.Managers.Identity.RoleClaims
{
    public interface IRoleClaimManager : IManager
    {
        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsAsync();

        Task<IResult<List<RoleClaimResponse>>> GetRoleClaimsByRoleIdAsync(string roleId);

        Task<IResult<string>> SaveAsync(RoleClaimRequest role);

        Task<IResult<string>> DeleteAsync(string id);
    }
}