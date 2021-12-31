using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Features.vbms.vehicle.vehicleTypeAttributes.Commands.Delete
{
    public class DeleteVehicleTypeAttributeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteVehicleTypeAttributeCommandHandler : IRequestHandler<DeleteVehicleTypeAttributeCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteVehicleTypeAttributeCommandHandler> _localizer;

        public DeleteVehicleTypeAttributeCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteVehicleTypeAttributeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteVehicleTypeAttributeCommand command, CancellationToken cancellationToken)
        {
            var vehicleTypeAttribute = await _unitOfWork.Repository<VehicleTypeAttribute>().GetByIdAsync(command.Id);
            if (vehicleTypeAttribute != null)
            {
                await _unitOfWork.Repository<VehicleTypeAttribute>().DeleteAsync(vehicleTypeAttribute);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(vehicleTypeAttribute.Id, _localizer["VehicleTypeAttribute Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["VehicleTypeAttribute Not Found!"]);
            }
        }
    }
}