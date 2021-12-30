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
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.Export
{
    public class ExportVehicleTypesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportVehicleTypesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportVehicleTypesQueryHandler : IRequestHandler<ExportVehicleTypesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportVehicleTypesQueryHandler> _localizer;

        public ExportVehicleTypesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportVehicleTypesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportVehicleTypesQuery request, CancellationToken cancellationToken)
        {
            var vehicleTypeFilterSpec = new VehicleTypeFilterSpecification(request.SearchString);
            var vehicleTypes = await _unitOfWork.Repository<VehicleType>().Entities
                .Specify(vehicleTypeFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(vehicleTypes, mappers: new Dictionary<string, Func<VehicleType, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Name"], item => item.Name },
                { _localizer["Description"], item => item.Description }
            }, sheetName: _localizer["VehicleTypes"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}
