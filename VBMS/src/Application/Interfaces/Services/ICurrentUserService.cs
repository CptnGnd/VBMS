using VBMS.Application.Interfaces.Common;

namespace VBMS.Application.Interfaces.Services
{
    public interface ICurrentUserService : IService
    {
        string UserId { get; }
    }
}