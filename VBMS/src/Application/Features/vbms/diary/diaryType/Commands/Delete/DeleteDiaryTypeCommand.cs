using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using VBMS.Domain.Entities.vbms.diary;

namespace VBMS.Application.Features.vbms.diary.diaryType.Commands.Delete
{
    public class DeleteDiaryTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteDiaryTypeCommandHandler : IRequestHandler<DeleteDiaryTypeCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteDiaryTypeCommandHandler> _localizer;

        public DeleteDiaryTypeCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteDiaryTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDiaryTypeCommand command, CancellationToken cancellationToken)
        {
            var diaryType = await _unitOfWork.Repository<DiaryType>().GetByIdAsync(command.Id);
            if (diaryType != null)
            {
                await _unitOfWork.Repository<DiaryType>().DeleteAsync(diaryType);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(diaryType.Id, _localizer["DiaryType Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["DiaryType Not Found!"]);
            }
        }
    }
}