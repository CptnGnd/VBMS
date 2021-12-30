using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.partner.partnerType.Commands.AddEdit;
using VBMS.Application.Features.vbms.partner.partnerType.Queries.GetAll;
using VBMS.Client.Infrastructure.Extensions;
using VBMS.Shared.Wrapper;

namespace VBMS.Client.Infrastructure.Managers.vbms.partner.partnerType
{
    public class PartnerTypeManager : IPartnerTypeManager
    {
        private readonly HttpClient _httpClient;

        public PartnerTypeManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.PartnerTypesEndpoints.Export
                : Routes.PartnerTypesEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.PartnerTypesEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<List<GetAllPartnerTypesResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.PartnerTypesEndpoints.GetAll);
            return await response.ToResult<List<GetAllPartnerTypesResponse>>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditPartnerTypeCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PartnerTypesEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}