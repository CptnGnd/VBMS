using VBMS.Application.Features.VehicleTypeAttributes.Commands.AddEdit;
using VBMS.Application.Features.VehicleTypeAttributes.Queries.GetAllPaged;
using VBMS.Shared.Wrapper;
using System.Threading.Tasks;
using VBMS.Application.Requests.vbms.vehicle;

namespace VBMS.Client.Infrastructure.Managers.vbms.vehicle.vehicleTypeAttribute
{
    public interface IVehicleTypeAttributeManager : IManager
    {
        Task<PaginatedResult<GetAllPagedVehicleTypeAttributesResponse>> GetVehicleTypeAttributesAsync(GetAllPagedVehicleTypeAttributesRequest request);

        //Task<IResult<string>> GetVehicleTypeAttributeImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditVehicleTypeAttributeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}