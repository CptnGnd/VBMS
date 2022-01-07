using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.booking.booking.Commands.AddEdit;
using VBMS.Application.Features.vbms.booking.booking.Queries.GetAll;
using VBMS.Application.Features.vbms.booking.booking.Queries.GetAllPaged;
using VBMS.Application.Requests.vbms.booking;
using VBMS.Client.Infrastructure.Extensions;
using VBMS.Shared.Wrapper;

namespace VBMS.Client.Infrastructure.Managers.vbms.bookings
{
    public class BookingManager : IBookingManager
    {
        private readonly HttpClient _httpClient;

        public BookingManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<int>> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"{Routes.BookingsEndpoints.Delete}/{id}");
            return await response.ToResult<int>();
        }

        public async Task<IResult<string>> ExportToExcelAsync(string searchString = "")
        {
            var response = await _httpClient.GetAsync(string.IsNullOrWhiteSpace(searchString)
                ? Routes.BookingsEndpoints.Export
                : Routes.BookingsEndpoints.ExportFiltered(searchString));
            return await response.ToResult<string>();
        }

        public async Task<IResult<List<GetAllBookingsResponse>>> GetAllAsync()
        {
            var response = await _httpClient.GetAsync(Routes.BookingsEndpoints.GetAll);
            return await response.ToResult<List<GetAllBookingsResponse>>();
        }

        public async Task<IResult<string>> GetBookingImageAsync(int id)
        {
            var response = await _httpClient.GetAsync(Routes.BookingsEndpoints.GetBookingImage(id));
            return await response.ToResult<string>();
        }

        public async Task<PaginatedResult<GetAllPagedBookingsResponse>> GetBookingsAsync(GetAllPagedBookingsRequest request)
        {
            var response = await _httpClient.GetAsync(Routes.BookingsEndpoints.GetAllPaged(request.PageNumber, request.PageSize, request.SearchString, request.Orderby));
            return await response.ToPaginatedResult<GetAllPagedBookingsResponse>();
        }

        public async Task<IResult<int>> SaveAsync(AddEditBookingCommand request)
        {
            var response = await _httpClient.PostAsJsonAsync(Routes.BookingsEndpoints.Save, request);
            return await response.ToResult<int>();
        }
    }
}