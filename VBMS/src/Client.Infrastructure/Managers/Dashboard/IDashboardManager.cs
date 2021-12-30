using VBMS.Shared.Wrapper;
using System.Threading.Tasks;
using VBMS.Application.Features.Dashboards.Queries.GetData;

namespace VBMS.Client.Infrastructure.Managers.Dashboard
{
    public interface IDashboardManager : IManager
    {
        Task<IResult<DashboardDataResponse>> GetDataAsync();
    }
}