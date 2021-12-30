using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.partners;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.partner.partner.Queries.GetProductImage
{
    public class GetPartnerImageQuery : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public GetPartnerImageQuery(int productTestId)
        {
            Id = productTestId;
        }
    }

    internal class GetPartnerImageQueryHandler : IRequestHandler<GetPartnerImageQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetPartnerImageQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GetPartnerImageQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<Partner>().Entities.Where(p => p.Id == request.Id).Select(a => a.ImageDataURL).FirstOrDefaultAsync(cancellationToken);
            return await Result<string>.SuccessAsync(data: data);
        }
    }
}