using VBMS.Client.Infrastructure.Extensions;
using VBMS.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.GetAll;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Commands.AddEdit;

namespace VBMS.Client.Infrastructure.Managers.vbms.vehicle.vehicleType
{
    public class VehicleTypeManager : IVehicleTypeManager
    {
        private readonly HttpClient _httpClient;

        public VehicleTypeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.VehicleTypesEndpoints.Export
                : Routes.VehicleTypesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.VehicleTypesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllVehicleTypesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.VehicleTypesEndpoints.GetAll);
            return await response.ToResult<List<GetAllVehicleTypesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditVehicleTypeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.VehicleTypesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}