using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VBMS.Application.Features.ProductTests.Queries.GetProductTestImage
{
    public class GetProductTestImageQuery : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public GetProductTestImageQuery(int productTestId)
        {
            Id = productTestId;
        }
    }

    internal class GetProductTestImageQueryHandler : IRequestHandler<GetProductTestImageQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetProductTestImageQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GetProductTestImageQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<ProductTest>().Entities.Where(p => p.Id == request.Id).Select(a => a.ImageDataURL).FirstOrDefaultAsync(cancellationToken);
            return await Result<string>.SuccessAsync(data: data);
        }
    }
}