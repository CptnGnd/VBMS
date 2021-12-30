using VBMS.Shared.Wrapper;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetAllPaged;
using VBMS.Application.Features.vbms.vehicle.vehicle.Commands.AddEdit;
using VBMS.Application.Requests.vbms.vehicle;

namespace VBMS.Client.Infrastructure.Managers.Catalog.Vehicle
{
    public interface IVehicleManager : IManager
    {
        Task<PaginatedResult<GetAllPagedVehiclesResponse>> GetVehiclesAsync(GetAllPagedVehiclesRequest request);

        Task<IResult<string>> GetVehicleImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditVehicleCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}