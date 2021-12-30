using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using VBMS.Shared.Constants.Application;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Features.vbms.vehicle.vehicleType.Commands.Delete
{
    public class DeleteVehicleTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteVehicleTypeCommandHandler : IRequestHandler<DeleteVehicleTypeCommand, Result<int>>
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IStringLocalizer<DeleteVehicleTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteVehicleTypeCommandHandler(IUnitOfWork<int> unitOfWork, IVehicleRepository vehicleRepository, IStringLocalizer<DeleteVehicleTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _vehicleRepository = vehicleRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteVehicleTypeCommand command, CancellationToken cancellationToken)
        {
            var isVehicleTypeUsed = await _vehicleRepository.IsVehicleTypeUsed(command.Id);
            if (!isVehicleTypeUsed)
            {
                var vehicleType = await _unitOfWork.Repository<VehicleType>().GetByIdAsync(command.Id);
                if (vehicleType != null)
                {
                    await _unitOfWork.Repository<VehicleType>().DeleteAsync(vehicleType);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllVehicleTypesCacheKey);
                    return await Result<int>.SuccessAsync(vehicleType.Id, _localizer["VehicleType Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["VehicleType Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}