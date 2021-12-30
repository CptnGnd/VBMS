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

namespace VBMS.Application.Features.vbms.partner.partner.Queries.Export
{
    public class ExportPartnersQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportPartnersQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportPartnersQueryHandler : IRequestHandler<ExportPartnersQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportPartnersQueryHandler> _localizer;

        public ExportPartnersQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportPartnersQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportPartnersQuery request, CancellationToken cancellationToken)
        {
            var partnerFilterSpec = new PartnerFilterSpecification(request.SearchString);
            var partners = await _unitOfWork.Repository<Partner>().Entities
                .Specify(partnerFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(partners, mappers: new Dictionary<string, Func<Partner, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["ShortName"], item => item.ShortName },
                { _localizer["PartnerType"], item => item.PartnerType.Name },
                { _localizer["PartnerTypeId"], item => item.PartnerTypeId }
            }, sheetName: _localizer["Partners"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}