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

namespace VBMS.Application.Features.vbms.vehicle.vehicle.Queries.Export
{
    public class ExportVehiclesQuery : IRequest<Result<string>>
    {
        public string SearchString { get; set; }

        public ExportVehiclesQuery(string searchString = "")
        {
            SearchString = searchString;
        }
    }

    internal class ExportVehiclesQueryHandler : IRequestHandler<ExportVehiclesQuery, Result<string>>
    {
        private readonly IExcelService _excelService;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<ExportVehiclesQueryHandler> _localizer;

        public ExportVehiclesQueryHandler(IExcelService excelService
            , IUnitOfWork<int> unitOfWork
            , IStringLocalizer<ExportVehiclesQueryHandler> localizer)
        {
            _excelService = excelService;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<string>> Handle(ExportVehiclesQuery request, CancellationToken cancellationToken)
        {
            var vehicleFilterSpec = new VehicleFilterSpecification(request.SearchString);
            var vehicles = await _unitOfWork.Repository<Vehicle>().Entities
                .Specify(vehicleFilterSpec)
                .ToListAsync(cancellationToken);
            var data = await _excelService.ExportAsync(vehicles, mappers: new Dictionary<string, Func<Vehicle, object>>
            {
                { _localizer["Id"], item => item.Id },
                { _localizer["Rego"], item => item.Rego },
                { _localizer["Description"], item => item.Description },
                { _localizer["Type"], item => item.VehicleType.Name }
            }, sheetName: _localizer["Vehicles"]);

            return await Result<string>.SuccessAsync(data: data);
        }
    }
}