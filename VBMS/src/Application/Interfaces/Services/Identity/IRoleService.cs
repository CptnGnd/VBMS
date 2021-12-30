using VBMS.Application.Interfaces.Common;
using VBMS.Application.Requests.Identity;
using VBMS.Application.Responses.Identity;
using VBMS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VBMS.Application.Interfaces.Services.Identity
{
    public interface IRoleService : IService
    {
        Task<Result<List<RoleResponse>>> GetAllAsync();

        Task<int> GetCountAsync();

        Task<Result<RoleResponse>> GetByIdAsync(string id);

        Task<Result<string>> SaveAsync(RoleRequest request);

        Task<Result<string>> DeleteAsync(string id);

        Task<Result<PermissionResponse>> GetAllPermissionsAsync(string roleId);

        Task<Result<string>> UpdatePermissionsAsync(PermissionRequest request);
    }
}