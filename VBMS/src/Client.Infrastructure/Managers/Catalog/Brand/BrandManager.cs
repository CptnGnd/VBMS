using VBMS.Application.Features.BrandTests.Queries.GetAll;
using VBMS.Client.Infrastructure.Extensions;
using VBMS.Shared.Wrapper;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VBMS.Application.Features.BrandTests.Commands.AddEdit;

namespace VBMS.Client.Infrastructure.Managers.Catalog.BrandTest
{
    public class BrandTestManager : IBrandTestManager
    {
        private readonly HttpClient _httpClient;

        public BrandTestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.BrandTestsEndpoints.Export
                : Routes.BrandTestsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.BrandTestsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllVehicleTypessResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.BrandTestsEndpoints.GetAll);
            return await response.ToResult<List<GetAllVehicleTypessResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditBrandTestCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.BrandTestsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}