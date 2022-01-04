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
using VBMS.Domain.Entities.vbms.diary;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.diary.dairyType.Commands.AddEdit
{
    public partial class AddEditDairyTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Colour { get; set; }
    }

    internal class AddEditDairyTypeCommandHandler : IRequestHandler<AddEditDairyTypeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditDairyTypeCommandHandler> _localizer;

        public AddEditDairyTypeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditDairyTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditDairyTypeCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<DairyType>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Name == command.Name, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["DairyType already exists."]);
            }
            if (await _unitOfWork.Repository<DairyType>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Color == command.Colour, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Colour Must Be Unique."]);
            }

            //var uploadRequest = command.UploadRequest;
            //if (uploadRequest != null)
            //{
            //    uploadRequest.FileName = $"P-{command.Barcode}{uploadRequest.Extension}";
            //}

            if (command.Id == 0)
            {
                var dairyType = _mapper.Map<DairyType>(command);
                //if (uploadRequest != null)
                //{
                //    dairyType.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                //}
                await _unitOfWork.Repository<DairyType>().AddAsync(dairyType);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(dairyType.Id, _localizer["DairyType Saved"]);
            }
            else
            {
                var dairyType = await _unitOfWork.Repository<DairyType>().GetByIdAsync(command.Id);
                if (dairyType != null)
                {
                    dairyType.Name = command.Name ?? dairyType.Name;
                    dairyType.Description = command.Description ?? dairyType.Description;
                    dairyType.Color = command.Colour ?? dairyType.Color;
                    await _unitOfWork.Repository<DairyType>().UpdateAsync(dairyType);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(dairyType.Id, _localizer["DairyType Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["DairyType Not Found!"]);
                }
            }
        }
    }
}