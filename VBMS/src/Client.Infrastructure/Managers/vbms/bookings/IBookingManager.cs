using System.Threading.Tasks;
using VBMS.Application.Features.vbms.booking.booking.Commands.AddEdit;
using VBMS.Application.Features.vbms.booking.booking.Queries.GetAllPaged;
using VBMS.Application.Requests.vbms.booking;
using VBMS.Shared.Wrapper;

namespace VBMS.Client.Infrastructure.Managers.vbms.bookings
{
    public interface IBookingManager : IManager
    {
        Task<PaginatedResult<GetAllPagedBookingsResponse>> GetBookingsAsync(GetAllPagedBookingsRequest request);

        Task<IResult<string>> GetBookingImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditBookingCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}