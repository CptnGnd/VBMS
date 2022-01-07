using MediatR;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Extensions;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Specifications.Catalog;
using VBMS.Domain.Entities.vbms.diary;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.diary.diary.Queries.GetAllPaged
{
    public class GetAllDiarysQuery : IRequest<PaginatedResult<GetAllPagedDiarysResponse>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchString { get; set; }
        public string[] OrderBy { get; set; } // of the form fieldname [ascending|descending],fieldname [ascending|descending]...

        public GetAllDiarysQuery(int pageNumber, int pageSize, string searchString, string orderBy)
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

    internal class GetAllDiarysQueryHandler : IRequestHandler<GetAllDiarysQuery, PaginatedResult<GetAllPagedDiarysResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetAllDiarysQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedResult<GetAllPagedDiarysResponse>> Handle(GetAllDiarysQuery request, CancellationToken cancellationToken)
        {
            Expression<Func<Diary, GetAllPagedDiarysResponse>> expression = e => new GetAllPagedDiarysResponse
            {
                Id = e.Id,
                StartDatTime = e.StartDateTime,
                EndDatTime = e.EndDateTime,
                DiaryTypeId = e.DiaryTypeId,
                DiaryType = e.DiaryType.Name,
                BookingId = e.BookingId,
                Booking = e.Booking.BookingCode,
                VehicleId = e.VehicleId,
                Vehicle = e.Vehicle.Rego
            };
            var diaryFilterSpec = new DiaryFilterSpecification(request.SearchString);
            if (request.OrderBy?.Any() != true)
            {
                var data = await _unitOfWork.Repository<Diary>().Entities
                   .Specify(diaryFilterSpec)
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;
            }
            else
            {
                var ordering = string.Join(",", request.OrderBy); // of the form fieldname [ascending|descending], ...
                var data = await _unitOfWork.Repository<Diary>().Entities
                   .Specify(diaryFilterSpec)
                   .OrderBy(ordering) // require system.linq.dynamic.core
                   .Select(expression)
                   .ToPaginatedListAsync(request.PageNumber, request.PageSize);
                return data;

            }
        }
    }
}