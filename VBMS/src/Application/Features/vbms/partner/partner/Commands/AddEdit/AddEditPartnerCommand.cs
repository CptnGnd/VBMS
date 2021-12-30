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
using VBMS.Application.Requests;
using VBMS.Domain.Entities.vbms.partners;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.partner.partner.Commands.AddEdit
{
    public partial class AddEditPartnerCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string ShortName { get; set; }
        public string ImageDataURL { get; set; }
        [Required]
        public int PartnerTypeId { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditPartnerCommandHandler : IRequestHandler<AddEditPartnerCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditPartnerCommandHandler> _localizer;

        public AddEditPartnerCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditPartnerCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditPartnerCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Partner>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Name == command.Name, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Barcode already exists."]);
            }

            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"P-{command.ShortName}{uploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                var productTest = _mapper.Map<Partner>(command);
                if (uploadRequest != null)
                {
                    productTest.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                }
                await _unitOfWork.Repository<Partner>().AddAsync(productTest);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(productTest.Id, _localizer["Partner Saved"]);
            }
            else
            {
                var productTest = await _unitOfWork.Repository<Partner>().GetByIdAsync(command.Id);
                if (productTest != null)
                {
                    productTest.Name = command.Name ?? productTest.Name;
                    productTest.ShortName = command.ShortName ?? productTest.ShortName;
                    if (uploadRequest != null)
                    {
                        productTest.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                    }
                    productTest.PartnerTypeId = command.PartnerTypeId == 0 ? productTest.PartnerTypeId : command.PartnerTypeId;
                    await _unitOfWork.Repository<Partner>().UpdateAsync(productTest);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(productTest.Id, _localizer["Partner Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Partner Not Found!"]);
                }
            }
        }
    }
}