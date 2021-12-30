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
                return await Result<int>.FailAsync(_localizer["Barcode already exists."]);
            }

            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"P-{command.Rego}{uploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                var productTest = _mapper.Map<Vehicle>(command);
                if (uploadRequest != null)
                {
                    productTest.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                }
                await _unitOfWork.Repository<Vehicle>().AddAsync(productTest);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(productTest.Id, _localizer["Vehicle Saved"]);
            }
            else
            {
                var productTest = await _unitOfWork.Repository<Vehicle>().GetByIdAsync(command.Id);
                if (productTest != null)
                {
                    productTest.Rego = command.Rego ?? productTest.Rego;
                    productTest.Description = command.Description ?? productTest.Description;
                    if (uploadRequest != null)
                    {
                        productTest.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                    }
                    productTest.VehicleTypeId = command.VehicleTypeId == 0 ? productTest.VehicleTypeId : command.VehicleTypeId;
                    await _unitOfWork.Repository<Vehicle>().UpdateAsync(productTest);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(productTest.Id, _localizer["Vehicle Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Vehicle Not Found!"]);
                }
            }
        }
    }
}