using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.diary.diaryType.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.diaryType.Queries.GetAll;
using VBMS.Client.Infrastructure.Extensions;
using VBMS.Shared.Wrapper;

namespace VBMS.Client.Infrastructure.Managers.vbms.diary.diaryType
{
    public class DiaryTypeManager : IDiaryTypeManager
    {
        private readonly HttpClient _httpClient;

        public DiaryTypeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.DiaryTypesEndpoints.Export
                : Routes.DiaryTypesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.DiaryTypesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllDiaryTypesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.DiaryTypesEndpoints.GetAll);
            return await response.ToResult<List<GetAllDiaryTypesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditDiaryTypeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.DiaryTypesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}