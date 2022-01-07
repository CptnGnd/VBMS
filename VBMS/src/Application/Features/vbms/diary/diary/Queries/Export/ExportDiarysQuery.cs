using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Interfaces.Services;
using VBMS.Domain.Entities.Catalog;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Extensions;
using VBMS.Application.Specifications.Catalog;
using VBMS.Shared.Wrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using VBMS.Domain.Entities.vbms.diary;

namespace VBMS.Application.Features.Diarys.Queries.Export
{
    public class ExportDiarysQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportDiarysQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportDiarysQueryHandler : IRequestHandler<ExportDiarysQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportDiarysQueryHandler> _localizer;

        public ExportDiarysQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportDiarysQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportDiarysQuery request, CancellationToken cancellationToken)
        {
            var diaryFilterSpec = new DiaryFilterSpecification(request.SearchString);
            var diarys = await _unitOfWork.Repository<Diary>().Entities
                .Specify(diaryFilterSpec)
                .ToListAsync( cancellationToken);
            var data = await _excelService.ExportAsync(diarys, mappers: new Dictionary<string, Func<Diary, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Start"], item => item.StartDateTime },
                { _localizer["End"], item => item.EndDateTime },
                { _localizer["Type"], item => item.DiaryTypeId },
                { _localizer["Vehicle"], item => item.VehicleId },
                { _localizer["Booking"], item => item.BookingId }
            }, sheetName: _localizer["Diarys"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}