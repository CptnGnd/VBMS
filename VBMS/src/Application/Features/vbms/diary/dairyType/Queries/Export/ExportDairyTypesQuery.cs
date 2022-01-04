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
using VBMS.Domain.Entities.vbms.diary;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.diary.dairyType.Queries.Export
{
    public class ExportDairyTypesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportDairyTypesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportDairyTypesQueryHandler : IRequestHandler<ExportDairyTypesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportDairyTypesQueryHandler> _localizer;

        public ExportDairyTypesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportDairyTypesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportDairyTypesQuery request, CancellationToken cancellationToken)
        {
            var productTestFilterSpec = new DairyTypeFilterSpecification(request.SearchString);
            var productTests = await _unitOfWork.Repository<DairyType>().Entities
                .Specify(productTestFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(productTests, mappers: new Dictionary<string, Func<DairyType, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description },
                { _localizer["Colour"], item => item.Color }
            }, sheetName: _localizer["DairyTypes"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}