using System.ComponentModel.DataAnnotations;
using AutoMapper;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using VBMS.Shared.Constants.Application;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Features.vbms.vehicle.vehicleType.Commands.AddEdit
{
    public partial class AddEditVehicleTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
    }

    internal class AddEditVehicleTypeCommandHandler : IRequestHandler<AddEditVehicleTypeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditVehicleTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditVehicleTypeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditVehicleTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditVehicleTypeCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var vehicleType = _mapper.Map<VehicleType>(command);
                await _unitOfWork.Repository<VehicleType>().AddAsync(vehicleType);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllVehicleTypesCacheKey);
                return await Result<int>.SuccessAsync(vehicleType.Id, _localizer["VehicleType Saved"]);
            }
            else
            {
                var vehicleType = await _unitOfWork.Repository<VehicleType>().GetByIdAsync(command.Id);
                if (vehicleType != null)
                {
                    vehicleType.Name = command.Name ?? vehicleType.Name;
                    vehicleType.Description = command.Description ?? vehicleType.Description;
                    await _unitOfWork.Repository<VehicleType>().UpdateAsync(vehicleType);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllVehicleTypesCacheKey);
                    return await Result<int>.SuccessAsync(vehicleType.Id, _localizer["VehicleType Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["VehicleType Not Found!"]);
                }
            }
        }
    }
}