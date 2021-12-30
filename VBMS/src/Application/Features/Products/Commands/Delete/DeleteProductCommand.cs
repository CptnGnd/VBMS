using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;

namespace VBMS.Application.Features.ProductTests.Commands.Delete
{
    public class DeleteProductTestCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteProductTestCommandHandler : IRequestHandler<DeleteProductTestCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteProductTestCommandHandler> _localizer;

        public DeleteProductTestCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteProductTestCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteProductTestCommand command, CancellationToken cancellationToken)
        {
            var productTest = await _unitOfWork.Repository<ProductTest>().GetByIdAsync(command.Id);
            if (productTest != null)
            {
                await _unitOfWork.Repository<ProductTest>().DeleteAsync(productTest);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(productTest.Id, _localizer["ProductTest Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["ProductTest Not Found!"]);
            }
        }
    }
}