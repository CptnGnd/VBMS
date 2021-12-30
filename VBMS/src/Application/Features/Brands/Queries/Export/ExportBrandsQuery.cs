using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Extensions;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Interfaces.Services;
using VBMS.Application.Specifications.Catalog;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace VBMS.Application.Features.BrandTests.Queries.Export
{
    public class ExportBrandTestsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportBrandTestsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportBrandTestsQueryHandler : IRequestHandler<ExportBrandTestsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportBrandTestsQueryHandler> _localizer;

        public ExportBrandTestsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportBrandTestsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportBrandTestsQuery request, CancellationToken cancellationToken)
        {
            var brandTestFilterSpec = new BrandTestFilterSpecification(request.SearchString);
            var brandTests = await _unitOfWork.Repository<BrandTest>().Entities
                .Specify(brandTestFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(brandTests, mappers: new Dictionary<string, Func<BrandTest, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description },
                { _localizer["Tax"], item => item.Tax }
            }, sheetName: _localizer["BrandTests"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
