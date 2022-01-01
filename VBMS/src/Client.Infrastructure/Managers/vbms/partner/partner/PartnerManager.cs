using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.partner.partner.Commands.AddEdit;
using VBMS.Application.Features.vbms.partner.partner.Queries.GetAllPaged;
using VBMS.Application.Requests.vbms.partner;
using VBMS.Client.Infrastructure.Extensions;
using VBMS.Shared.Wrapper;

namespace VBMS.Client.Infrastructure.Managers.vbms.partner.partner
{
    public class PartnerManager : IPartnerManager
    {
        private readonly HttpClient _httpClient;

        public PartnerManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.PartnersEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.PartnersEndpoints.Export
                : Routes.PartnersEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        //public async Task<PaginatedResult<GetAllPagedPartnersResponse>> GetAllPartnersAsync(GetAllPagedPartnersRequest request)
        //{
        //    var response = await _httpClient.GetAsync(Routes.PartnersEndpoints.GetAll);
        //    return (PaginatedResult<GetAllPagedPartnersResponse>)await response.ToResult<List<GetAllPagedPartnersResponse>>();
        //    // throw new System.NotImplementedException();
        //}

        public async Task<PaginatedResult<GetAllPagedPartnersResponse>> GetAllPartnersAsync()
        {
                var response = await _httpClient.GetAsync(Routes.PartnersEndpoints.GetAll);
                return (PaginatedResult<GetAllPagedPartnersResponse>)await response.ToResult<List<GetAllPagedPartnersResponse>>();
            //    // throw new System.NotImplementedException();
        }

        public async Task<IResult<string>> GetPartnerImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.PartnersEndpoints.GetPartnerImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedPartnersResponse>> GetPartnersAsync(GetAllPagedPartnersRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.PartnersEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedPartnersResponse>();
        }


        public async Task<IResult<int>> SaveAsync(AddEditPartnerCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.PartnersEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}