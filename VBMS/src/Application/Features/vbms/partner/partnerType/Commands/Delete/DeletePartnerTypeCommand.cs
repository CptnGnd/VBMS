using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using VBMS.Shared.Constants.Application;
using VBMS.Domain.Entities.vbms.partners;

namespace VBMS.Application.Features.vbms.partner.partnerType.Commands.Delete
{
    public class DeletePartnerTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeletePartnerTypeCommandHandler : IRequestHandler<DeletePartnerTypeCommand, Result<int>>
    {
        private readonly IPartnerRepository _partnerRepository;
        private readonly IStringLocalizer<DeletePartnerTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeletePartnerTypeCommandHandler(IUnitOfWork<int> unitOfWork, IPartnerRepository partnerRepository, IStringLocalizer<DeletePartnerTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _partnerRepository = partnerRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeletePartnerTypeCommand command, CancellationToken cancellationToken)
        {
            var isPartnerTypeUsed = await _partnerRepository.IsPartnerTypeUsed(command.Id);
            if (!isPartnerTypeUsed)
            {
                var partnerType = await _unitOfWork.Repository<PartnerType>().GetByIdAsync(command.Id);
                if (partnerType != null)
                {
                    await _unitOfWork.Repository<PartnerType>().DeleteAsync(partnerType);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPartnerTypesCacheKey);
                    return await Result<int>.SuccessAsync(partnerType.Id, _localizer["PartnerType Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["PartnerType Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}