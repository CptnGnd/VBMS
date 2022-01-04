using AutoMapper;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace VBMS.Application.Features.BrandTests.Queries.GetById
{
    public class GetBrandTestByIdQuery : IRequest<Result<GetBrandTestByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductTestByIdQueryHandler : IRequestHandler<GetBrandTestByIdQuery, Result<GetBrandTestByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductTestByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetBrandTestByIdResponse>> Handle(GetBrandTestByIdQuery query, CancellationToken cancellationToken)
        {
            var brandTest = await _unitOfWork.Repository<BrandTest>().GetByIdAsync(query.Id);
            var mappedBrandTest = _mapper.Map<GetBrandTestByIdResponse>(brandTest);
            return await Result<GetBrandTestByIdResponse>.SuccessAsync(mappedBrandTest);
        }
    }
}