using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.partners;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.partner.partnerType.Queries.GetById
{
    public class GetPartnerTypeByIdQuery : IRequest<Result<GetPartnerTypeByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductTestByIdQueryHandler : IRequestHandler<GetPartnerTypeByIdQuery, Result<GetPartnerTypeByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductTestByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetPartnerTypeByIdResponse>> Handle(GetPartnerTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var partnerType = await _unitOfWork.Repository<PartnerType>().GetByIdAsync(query.Id);
            var mappedPartnerType = _mapper.Map<GetPartnerTypeByIdResponse>(partnerType);
            return await Result<GetPartnerTypeByIdResponse>.SuccessAsync(mappedPartnerType);
        }
    }
}