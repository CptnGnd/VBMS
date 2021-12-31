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
using VBMS.Domain.Entities.vbms.vehicles;
using VBMS.Application.Specifications.vbms;

namespace VBMS.Application.Features.vbms.vehicle.vehicleTypeAttributes.Queries.Export
{
    public class ExportVehicleTypeAttributesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportVehicleTypeAttributesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportVehicleTypeAttributesQueryHandler : IRequestHandler<ExportVehicleTypeAttributesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportVehicleTypeAttributesQueryHandler> _localizer;

        public ExportVehicleTypeAttributesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportVehicleTypeAttributesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportVehicleTypeAttributesQuery request, CancellationToken cancellationToken)
        {
            var vehicleTypeAttributeFilterSpec = new VehicleTypeAttributeFilterSpecification(request.SearchString);
            var vehicleTypeAttributes = await _unitOfWork.Repository<VehicleTypeAttribute>().Entities
                .Specify(vehicleTypeAttributeFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(vehicleTypeAttributes, mappers: new Dictionary<string, Func<VehicleTypeAttribute, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description },
                { _localizer["Type"], item => item.AttributeType },
                { _localizer["VehicleTypeId"], item => item.VehicleTypeId },
                { _localizer["VehicleType"], item => item.VehicleType.Name }

            }, sheetName: _localizer["VehicleTypeAttributes"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}