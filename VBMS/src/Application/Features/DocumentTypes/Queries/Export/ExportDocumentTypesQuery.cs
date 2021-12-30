using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Extensions;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Interfaces.Services;
using VBMS.Application.Specifications.Misc;
using VBMS.Domain.Entities.Misc;
using VBMS.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace VBMS.Application.Features.DocumentTypes.Queries.Export
{
    public class ExportDocumentTypesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportDocumentTypesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportDocumentTypesQueryHandler : IRequestHandler<ExportDocumentTypesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportDocumentTypesQueryHandler> _localizer;

        public ExportDocumentTypesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportDocumentTypesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportDocumentTypesQuery request, CancellationToken cancellationToken)
        {
            var documentTypeFilterSpec = new DocumentTypeFilterSpecification(request.SearchString);
            var documentTypes = await _unitOfWork.Repository<DocumentType>().Entities
                .Specify(documentTypeFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(documentTypes, mappers: new Dictionary<string, Func<DocumentType, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description }
            }, sheetName: _localizer["Document Types"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}