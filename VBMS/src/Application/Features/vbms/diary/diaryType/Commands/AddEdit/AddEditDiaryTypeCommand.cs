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

namespace VBMS.Application.Features.vbms.diary.diaryType.Commands.AddEdit
{
    public partial class AddEditDiaryTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Colour { get; set; }
    }

    internal class AddEditDiaryTypeCommandHandler : IRequestHandler<AddEditDiaryTypeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditDiaryTypeCommandHandler> _localizer;

        public AddEditDiaryTypeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditDiaryTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditDiaryTypeCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<DiaryType>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Name == command.Name, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["DiaryType already exists."]);
            }
            if (await _unitOfWork.Repository<DiaryType>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.Colour == command.Colour, cancellationToken))
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
                var diaryType = _mapper.Map<DiaryType>(command);
                //if (uploadRequest != null)
                //{
                //    diaryType.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                //}
                await _unitOfWork.Repository<DiaryType>().AddAsync(diaryType);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(diaryType.Id, _localizer["DiaryType Saved"]);
            }
            else
            {
                var diaryType = await _unitOfWork.Repository<DiaryType>().GetByIdAsync(command.Id);
                if (diaryType != null)
                {
                    diaryType.Name = command.Name ?? diaryType.Name;
                    diaryType.Description = command.Description ?? diaryType.Description;
                    diaryType.Colour = command.Colour ?? diaryType.Colour;
                    await _unitOfWork.Repository<DiaryType>().UpdateAsync(diaryType);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(diaryType.Id, _localizer["DiaryType Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["DiaryType Not Found!"]);
                }
            }
        }
    }
}