using AutoMapper;
using MediatR;
using Microsoft.Extensions.Localization;
using System.ComponentModel.DataAnnotations;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Constants.Application;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.BrandTests.Commands.AddEdit
{
    public partial class AddEditBrandTestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public decimal Tax { get; set; }
    }

    internal class AddEditBrandTestCommandHandler : IRequestHandler<AddEditBrandTestCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AddEditBrandTestCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public AddEditBrandTestCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditBrandTestCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditBrandTestCommand command, CancellationToken cancellationToken)
        {
            if (command.Id == 0)
            {
                var brandTest = _mapper.Map<BrandTest>(command);
                await _unitOfWork.Repository<BrandTest>().AddAsync(brandTest);
                await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBrandTestsCacheKey);
                return await Result<int>.SuccessAsync(brandTest.Id, _localizer["BrandTest Saved"]);
            }
            else
            {
                var brandTest = await _unitOfWork.Repository<BrandTest>().GetByIdAsync(command.Id);
                if (brandTest != null)
                {
                    brandTest.Name = command.Name ?? brandTest.Name;
                    brandTest.Tax = (command.Tax == 0) ? brandTest.Tax : command.Tax;
                    brandTest.Description = command.Description ?? brandTest.Description;
                    await _unitOfWork.Repository<BrandTest>().UpdateAsync(brandTest);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBrandTestsCacheKey);
                    return await Result<int>.SuccessAsync(brandTest.Id, _localizer["BrandTest Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["BrandTest Not Found!"]);
                }
            }
        }
    }
}