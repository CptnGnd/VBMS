using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using VBMS.Shared.Constants.Application;

namespace VBMS.Application.Features.BrandTests.Commands.Delete
{
    public class DeleteBrandTestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteBrandTestCommandHandler : IRequestHandler<DeleteBrandTestCommand, Result<int>>
    {
        private readonly IProductTestRepository _productTestRepository;
        private readonly IStringLocalizer<DeleteBrandTestCommandHandler> _localizer;
        private readonly IUnitOfWork<int> _unitOfWork;

        public DeleteBrandTestCommandHandler(IUnitOfWork<int> unitOfWork, IProductTestRepository productTestRepository, IStringLocalizer<DeleteBrandTestCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _productTestRepository = productTestRepository;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteBrandTestCommand command, CancellationToken cancellationToken)
        {
            var isBrandTestUsed = await _productTestRepository.IsBrandTestUsed(command.Id);
            if (!isBrandTestUsed)
            {
                var brandTest = await _unitOfWork.Repository<BrandTest>().GetByIdAsync(command.Id);
                if (brandTest != null)
                {
                    await _unitOfWork.Repository<BrandTest>().DeleteAsync(brandTest);
                    await _unitOfWork.CommitAndRemoveCache(cancellationToken, ApplicationConstants.Cache.GetAllBrandTestsCacheKey);
                    return await Result<int>.SuccessAsync(brandTest.Id, _localizer["BrandTest Deleted"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["BrandTest Not Found!"]);
                }
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Deletion Not Allowed"]);
            }
        }
    }
}