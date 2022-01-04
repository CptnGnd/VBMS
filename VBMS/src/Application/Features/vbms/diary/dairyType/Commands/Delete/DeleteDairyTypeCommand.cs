using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using VBMS.Domain.Entities.vbms.diary;

namespace VBMS.Application.Features.vbms.diary.dairyType.Commands.Delete
{
    public class DeleteDairyTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteDairyTypeCommandHandler : IRequestHandler<DeleteDairyTypeCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteDairyTypeCommandHandler> _localizer;

        public DeleteDairyTypeCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteDairyTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteDairyTypeCommand command, CancellationToken cancellationToken)
        {
            var dairyType = await _unitOfWork.Repository<DairyType>().GetByIdAsync(command.Id);
            if (dairyType != null)
            {
                await _unitOfWork.Repository<DairyType>().DeleteAsync(dairyType);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(dairyType.Id, _localizer["DairyType Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["DairyType Not Found!"]);
            }
        }
    }
}