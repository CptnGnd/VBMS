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

namespace VBMS.Application.Features.ProductTests.Queries.Export
{
    public class ExportProductTestsQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportProductTestsQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportProductTestsQueryHandler : IRequestHandler<ExportProductTestsQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportProductTestsQueryHandler> _localizer;

        public ExportProductTestsQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportProductTestsQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportProductTestsQuery request, CancellationToken cancellationToken)
        {
            var productTestFilterSpec = new ProductTestFilterSpecification(request.SearchString);
            var productTests = await _unitOfWork.Repository<ProductTest>().Entities
                .Specify(productTestFilterSpec)
                .ToListAsync( cancellationToken);
            var data = await _excelService.ExportAsync(productTests, mappers: new Dictionary<string, Func<ProductTest, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Barcode"], item => item.Barcode },
                { _localizer["Description"], item => item.Description },
                { _localizer["Rate"], item => item.Rate }
            }, sheetName: _localizer["ProductTests"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}