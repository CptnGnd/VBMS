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
using VBMS.Domain.Entities.vbms.partners;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.partner.partnerType.Queries.Export
{
    public class ExportPartnerTypesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportPartnerTypesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportPartnerTypesQueryHandler : IRequestHandler<ExportPartnerTypesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportPartnerTypesQueryHandler> _localizer;

        public ExportPartnerTypesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportPartnerTypesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportPartnerTypesQuery request, CancellationToken cancellationToken)
        {
            var brandTestFilterSpec = new PartnerTypeFilterSpecification(request.SearchString);
            var brandTests = await _unitOfWork.Repository<PartnerType>().Entities
                .Specify(brandTestFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(brandTests, mappers: new Dictionary<string, Func<PartnerType, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description }
            }, sheetName: _localizer["PartnerTypes"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
