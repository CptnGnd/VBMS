using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.partners;
using VBMS.Shared.Constants.Application;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.partner.partnerType.Commands.AddEdit
{
    public partial class AddEditPartnerTypeCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Tax { get; set; }
    }

    internal class AddEditPartnerTypeCommandHandler : IRequestHandler<AddEditPartnerTypeCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditPartnerTypeCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditPartnerTypeCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditPartnerTypeCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditPartnerTypeCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var partnerType = _mapper.Map<PartnerType>(command);
                await _unitOfWork.Repository<PartnerType>().AddAsync(partnerType);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPartnerTypesCacheKey);
                return await Result<int>.SuccessAsync(partnerType.Id, _localizer["PartnerType Saved"]);
            }
            else
            {
                var partnerType = await _unitOfWork.Repository<PartnerType>().GetByIdAsync(command.Id);
                if (partnerType != null)
                {
                    partnerType.Name = command.Name ?? partnerType.Name;
                    partnerType.Description = command.Description ?? partnerType.Description;
                    await _unitOfWork.Repository<PartnerType>().UpdateAsync(partnerType);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllPartnerTypesCacheKey);
                    return await Result<int>.SuccessAsync(partnerType.Id, _localizer["PartnerType Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["PartnerType Not Found!"]);
                }
            }
        }
    }
}