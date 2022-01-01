using MediatR;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Extensions;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Specifications.vbms;
using VBMS.Domain.Entities.vbms.bookings;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.booking.booking.Queries.GetAllPaged
{
    public class GetAllBookingsQuery : IRequest<PaginatedResult<GetAllPagedBookingsResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllBookingsQuery(int pageNumber, int pageSize, string searchString, string orderBy)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SearchString = searchString;
            if (!string.IsNullOrWhiteSpace(orderBy))
            {
                OrderBy = orderBy.Split(',');
            }
        }
    }

    internal class GetAllBookingsQueryHandler : IRequestHandler<GetAllBookingsQuery, PaginatedResult<GetAllPagedBookingsResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllBookingsQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedBookingsResponse>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Booking, GetAllPagedBookingsResponse>> expression = e => new GetAllPagedBookingsResponse
            {
                Id = e.Id,
                BookingCode = e.BookingCode,
                BookingType = e.BookingType,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                VehicleType = e.VehicleType.Name,
                VehicleTypeId = e.VehicleTypeId,
                Partner = e.Partner.Name,
                PartnerId = e.PartnerId
            };
            var productTestFilterSpec = new BookingFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Booking>().Entities
                   .Specify(productTestFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Booking>().Entities
                   .Specify(productTestFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}