using VBMS.Client.Infrastructure.Extensions;
using VBMS.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetAllPaged;
using VBMS.Application.Requests.vbms.vehicle;
using VBMS.Application.Features.vbms.vehicle.vehicle.Commands.AddEdit;
using VBMS.Client.Infrastructure.Managers.Catalog.Vehicle;
using System.Collections.Generic;
using VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetAll;

namespace VBMS.Client.Infrastructure.Managers.vbms.vehicle.vehicle
{
    public class VehicleManager : IVehicleManager
    {
        private readonly HttpClient _httpClient;

        public VehicleManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.VehiclesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.VehiclesEndpoints.Export
                : Routes.VehiclesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<GetAllVehiclesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.VehiclesEndpoints.GetAll);
            return await response.ToResult<List<GetAllVehiclesResponse>>();
        }

        public async Task<IResult<string>> GetVehicleImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.VehiclesEndpoints.GetVehicleImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedVehiclesResponse>> GetVehiclesAsync(GetAllPagedVehiclesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.VehiclesEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedVehiclesResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditVehicleCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.VehiclesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}