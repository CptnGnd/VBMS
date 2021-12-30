using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.partners;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.partner.partner.Commands.Delete
{
    public class DeletePartnerCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeletePartnerCommandHandler : IRequestHandler<DeletePartnerCommand, Result<int>>
    {
//todo Stop Deletion for partners with booking
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeletePartnerCommandHandler> _localizer;

        public DeletePartnerCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeletePartnerCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeletePartnerCommand command, CancellationToken cancellationToken)
        {
            var productTest = await _unitOfWork.Repository<Partner>().GetByIdAsync(command.Id);
            if (productTest != null)
            {
                await _unitOfWork.Repository<Partner>().DeleteAsync(productTest);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(productTest.Id, _localizer["Partner Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Partner Not Found!"]);
            }
        }
    }
}