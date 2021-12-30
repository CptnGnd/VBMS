using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Features.vbms.vehicle.vehicle.Commands.Delete
{
    public class DeleteVehicleCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteVehicleCommandHandler : IRequestHandler<DeleteVehicleCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteVehicleCommandHandler> _localizer;

        public DeleteVehicleCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteVehicleCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteVehicleCommand command, CancellationToken cancellationToken)
        {
            var vehicle = await _unitOfWork.Repository<Vehicle>().GetByIdAsync(command.Id);
            if (vehicle != null)
            {
                await _unitOfWork.Repository<Vehicle>().DeleteAsync(vehicle);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(vehicle.Id, _localizer["Vehicle Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Vehicle Not Found!"]);
            }
        }
    }
}