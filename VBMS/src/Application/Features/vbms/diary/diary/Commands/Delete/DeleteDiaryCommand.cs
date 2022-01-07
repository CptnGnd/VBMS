using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using VBMS.Domain.Entities.vbms.diary;

namespace VBMS.Application.Features.vbms.diary.diary.Commands.Delete
{
    public class DeleteDiaryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteDiaryCommandHandler : IRequestHandler<DeleteDiaryCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteDiaryCommandHandler> _localizer;

        public DeleteDiaryCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteDiaryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDiaryCommand command, CancellationToken cancellationToken)
        {
            var diary = await _unitOfWork.Repository<Diary>().GetByIdAsync(command.Id);
            if (diary != null)
            {
                await _unitOfWork.Repository<Diary>().DeleteAsync(diary);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(diary.Id, _localizer["Diary Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Diary Not Found!"]);
            }
        }
    }
}