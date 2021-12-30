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

namespace VBMS.Application.Features.ProductTests.Commands.AddEdit
{
    public partial class AddEditProductTestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        public string Description { get; set; }
        public string ImageDataURL { get; set; }
        [Required]
        public decimal Rate { get; set; }
        [Required]
        public int BrandTestId { get; set; }
        public UploadRequest UploadRequest { get; set; }
    }

    internal class AddEditProductTestCommandHandler : IRequestHandler<AddEditProductTestCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditProductTestCommandHandler> _localizer;

        public AddEditProductTestCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditProductTestCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditProductTestCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<ProductTest>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Barcode == command.Barcode, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Barcode already exists."]);
            }

            var uploadRequest = command.UploadRequest;
            if (uploadRequest != null)
            {
                uploadRequest.FileName = $"P-{command.Barcode}{uploadRequest.Extension}";
            }

            if (command.Id == 0)
            {
                var productTest = _mapper.Map<ProductTest>(command);
                if (uploadRequest != null)
                {
                    productTest.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                }
                await _unitOfWork.Repository<ProductTest>().AddAsync(productTest);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(productTest.Id, _localizer["ProductTest Saved"]);
            }
            else
            {
                var productTest = await _unitOfWork.Repository<ProductTest>().GetByIdAsync(command.Id);
                if (productTest != null)
                {
                    productTest.Name = command.Name ?? productTest.Name;
                    productTest.Description = command.Description ?? productTest.Description;
                    if (uploadRequest != null)
                    {
                        productTest.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                    }
                    productTest.Rate = (command.Rate == 0) ? productTest.Rate : command.Rate;
                    productTest.BrandTestId = (command.BrandTestId == 0) ? productTest.BrandTestId : command.BrandTestId;
                    await _unitOfWork.Repository<ProductTest>().UpdateAsync(productTest);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(productTest.Id, _localizer["ProductTest Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["ProductTest Not Found!"]);
                }
            }
        }
    }
}