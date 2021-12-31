using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Interfaces.Services;
using VBMS.Domain.Entities.vbms.vehicles;
using VBMS.Domain.Enums;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.VehicleTypeAttributes.Commands.AddEdit
{
    public partial class AddEditVehicleTypeAttributeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int VehicleTypeId { get; set; }
        [Required]
        public AttributeType AttributeType { get; set; }
    }

    internal class AddEditVehicleTypeAttributeCommandHandler : IRequestHandler<AddEditVehicleTypeAttributeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditVehicleTypeAttributeCommandHandler> _localizer;

        public AddEditVehicleTypeAttributeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditVehicleTypeAttributeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditVehicleTypeAttributeCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<VehicleTypeAttribute>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.VehicleTypeId == command.VehicleTypeId && p.Name == command.Name, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Attribute already exists."]);
            }

            //var uploadRequest = command.UploadRequest;
            //if (uploadRequest != null)
            //{
            //    uploadRequest.FileName = $"P-{command.Barcode}{uploadRequest.Extension}";
            //}

            if (command.Id == 0)
            {
                var vehicleTypeAttribute = _mapper.Map<VehicleTypeAttribute>(command);
                //if (uploadRequest != null)
                //{
                //    vehicleTypeAttribute.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                //}
                await _unitOfWork.Repository<VehicleTypeAttribute>().AddAsync(vehicleTypeAttribute);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(vehicleTypeAttribute.Id, _localizer["VehicleTypeAttribute Saved"]);
            }
            else
            {
                var vehicleTypeAttribute = await _unitOfWork.Repository<VehicleTypeAttribute>().GetByIdAsync(command.Id);
                if (vehicleTypeAttribute != null)
                {
                    vehicleTypeAttribute.Name = command.Name ?? vehicleTypeAttribute.Name;
                    vehicleTypeAttribute.Description = command.Description ?? vehicleTypeAttribute.Description;
                    //if (uploadRequest != null)
                    //{
                    //    vehicleTypeAttribute.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                    //}
                    vehicleTypeAttribute.AttributeType = (command.AttributeType == 0) ? vehicleTypeAttribute.AttributeType : command.AttributeType;
                    vehicleTypeAttribute.VehicleTypeId = (command.VehicleTypeId == 0) ? vehicleTypeAttribute.VehicleTypeId : command.VehicleTypeId;
                    await _unitOfWork.Repository<VehicleTypeAttribute>().UpdateAsync(vehicleTypeAttribute);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(vehicleTypeAttribute.Id, _localizer["VehicleTypeAttribute Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["VehicleTypeAttribute Not Found!"]);
                }
            }
        }
    }
}