using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.diary.diary.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.diary.Queries.GetAllPaged;
using VBMS.Application.Requests.vbms.diary;
using VBMS.Client.Infrastructure.Extensions;
using VBMS.Client.Infrastructure.Managers.Catalog.Diary;
using VBMS.Shared.Wrapper;

namespace VBMS.Client.Infrastructure.Managers.vbms.diary.diary
{
    public class DiaryManager : IDiaryManager
    {
        private readonly HttpClient _httpClient;

        public DiaryManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DiarysEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.DiarysEndpoints.Export
                : Routes.DiarysEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        //public async Task<IResult<string>> GetDiaryImageAsync(int id)
        //{
        //    var response = await _httpClient.GetAsync(Routes.DiarysEndpoints.GetDiaryImage(id));
        //    return await response.ToResult<string>();
        //}

        public async Task<PaginatedResult<GetAllPagedDiarysResponse>> GetDiarysAsync(GetAllPagedDiarysRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.DiarysEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedDiarysResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDiaryCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DiarysEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}