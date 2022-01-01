using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Extensions;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Interfaces.Services;
using VBMS.Application.Specifications.vbms;
using VBMS.Domain.Entities.vbms.bookings;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.booking.booking.Queries.Export
{
    public class ExportBookingsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportBookingsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportBookingsQueryHandler : IRequestHandler<ExportBookingsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportBookingsQueryHandler> _localizer;

        public ExportBookingsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportBookingsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportBookingsQuery request, CancellationToken cancellationToken)
        {
            var bookingFilterSpec = new BookingFilterSpecification(request.SearchString);
            var bookings = await _unitOfWork.Repository<Booking>().Entities
                .Specify(bookingFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(bookings, mappers: new Dictionary<string, Func<Booking, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.BookingCode },
                { _localizer["Barcode"], item => item.BookingType },
                { _localizer["Description"], item => item.PartnerId },
                { _localizer["Description"], item => item.Partner.Name },
                { _localizer["Description"], item => item.VehicleTypeId },
                { _localizer["Description"], item => item.VehicleType.Name },
                { _localizer["Rate"], item => item.StartDate },
                { _localizer["Rate"], item => item.EndDate }
            }, sheetName: _localizer["Bookings"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}