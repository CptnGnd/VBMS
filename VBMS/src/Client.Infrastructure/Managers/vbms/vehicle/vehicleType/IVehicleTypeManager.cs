using VBMS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.GetAll;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Commands.AddEdit;

namespace VBMS.Client.Infrastructure.Managers.vbms.vehicle.vehicleType
{
    public interface IVehicleTypeManager : IManager
    {
        Task<IResult<List<GetAllVehicleTypesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditVehicleTypeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}