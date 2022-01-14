using System.ComponentModel.DataAnnotations;
using AutoMapper;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Interfaces.Services;
using VBMS.Application.Requests;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VBMS.Domain.Entities.vbms.vehicles;
using VBMS.Domain.Entities.vbms.diary;
using System;

namespace VBMS.Application.Features.vbms.vehicle.vehicle.Commands.AddEdit
{
    public partial class AddEditVehicleCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Rego { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImageDataURL { get; set; }
        [Required]
        public int VehicleTypeId { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditVehicleCommandHandler : IRequestHandler<AddEditVehicleCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditVehicleCommandHandler> _localizer;

        public AddEditVehicleCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditVehicleCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditVehicleCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Vehicle>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Rego == command.Rego, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Vehicle with Rego already exists."]);
            }

            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"P-{command.Rego}{uploadRequest.Extension}";
            }
            if (command.Id == 0)
            {
                var vehicle = _mapper.Map<Vehicle>(command);
                Diary diary = new();
                if (uploadRequest != null)
                {
                    vehicle.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                }
                await _unitOfWork.Repository<Vehicle>().AddAsync(vehicle);
                await _unitOfWork.Commit(cancellationToken);
                diary.Id = 0;
                diary.DiaryTypeId = 1;
                diary.VehicleId = vehicle.Id;
                diary.StartDateTime = DateTime.Now;
                diary.EndDateTime = DateTime.MaxValue;
                diary.BookingId = null;
                diary.CreatedBy = vehicle.CreatedBy;
                await _unitOfWork.Repository<Diary>().AddAsync(diary);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(vehicle.Id, _localizer["Vehicle Saved"]);
            }
            else
            {
                var vehicle = await _unitOfWork.Repository<Vehicle>().GetByIdAsync(command.Id);
                if (vehicle != null)
                {
                    vehicle.Rego = command.Rego ?? vehicle.Rego;
                    vehicle.Description = command.Description ?? vehicle.Description;
                    if (uploadRequest != null)
                    {
                        vehicle.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                    }
                    vehicle.VehicleTypeId = command.VehicleTypeId == 0 ? vehicle.VehicleTypeId : command.VehicleTypeId;
                    await _unitOfWork.Repository<Vehicle>().UpdateAsync(vehicle);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(vehicle.Id, _localizer["Vehicle Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Vehicle Not Found!"]);
                }
            }
        }
    }
}