using VBMS.Application.Features.VehicleTypeAttributes.Commands.AddEdit;
using VBMS.Application.Features.VehicleTypeAttributes.Queries.GetAllPaged;
using VBMS.Application.Requests.Catalog;
using VBMS.Client.Infrastructure.Extensions;
using VBMS.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VBMS.Application.Requests.vbms.vehicle;

namespace VBMS.Client.Infrastructure.Managers.vbms.vehicle.vehicleTypeAttribute
{
    public class VehicleTypeAttributeManager : IVehicleTypeAttributeManager
    {
        private readonly HttpClient _httpClient;

        public VehicleTypeAttributeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.VehicleTypeAttributesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.VehicleTypeAttributesEndpoints.Export
                : Routes.VehicleTypeAttributesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        //public async Task<IResult<string>> GetVehicleTypeAttributeImageAsync(int id)
        //{
        //    var response = await _httpClient.GetAsync(Routes.VehicleTypeAttributesEndpoints.GetVehicleTypeAttributeImage(id));
        //    return await response.ToResult<string>();
        //}

        public async Task<PaginatedResult<GetAllPagedVehicleTypeAttributesResponse>> GetVehicleTypeAttributesAsync(GetAllPagedVehicleTypeAttributesRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.VehicleTypeAttributesEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedVehicleTypeAttributesResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditVehicleTypeAttributeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.VehicleTypeAttributesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}