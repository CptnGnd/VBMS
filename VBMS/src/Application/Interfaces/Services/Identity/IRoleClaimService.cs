using System.Collections.Generic;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Common;
using VBMS.Application.Requests.Identity;
using VBMS.Application.Responses.Identity;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Interfaces.Services.Identity
{
    public interface IRoleClaimService : IService
    {
        Task<Result<List<RoleClaimResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleClaimResponse>> GetByIdAsync(int id);

        Task<Result<List<RoleClaimResponse>>> GetAllByRoleIdAsync(string roleId);

        Task<Result<string>> SaveAsync(RoleClaimRequest request);

        Task<Result<string>> DeleteAsync(int id);
    }
}