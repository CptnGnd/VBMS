using VBMS.Application.Features.ProductTests.Commands.AddEdit;
using VBMS.Application.Features.ProductTests.Queries.GetAllPaged;
using VBMS.Application.Requests.Catalog;
using VBMS.Client.Infrastructure.Extensions;
using VBMS.Shared.Wrapper;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace VBMS.Client.Infrastructure.Managers.Catalog.ProductTest
{
    public class ProductTestManager : IProductTestManager
    {
        private readonly HttpClient _httpClient;

        public ProductTestManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.ProductTestsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.ProductTestsEndpoints.Export
                : Routes.ProductTestsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<string>> GetProductTestImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.ProductTestsEndpoints.GetProductTestImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedProductTestsResponse>> GetProductTestsAsync(GetAllPagedProductTestsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.ProductTestsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedProductTestsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditProductTestCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.ProductTestsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}