using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.diary;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.diary.dairyType.Queries.GetById
{
    public class GetDairyTypeByIdQuery : IRequest<Result<GetDairyTypeByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetDairyTypeByIdQueryHandler : IRequestHandler<GetDairyTypeByIdQuery, Result<GetDairyTypeByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetDairyTypeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetDairyTypeByIdResponse>> Handle(GetDairyTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var dairyType = await _unitOfWork.Repository<DairyType>().GetByIdAsync(query.Id);
            var mappedDairyType = _mapper.Map<GetDairyTypeByIdResponse>(dairyType);
            return await Result<GetDairyTypeByIdResponse>.SuccessAsync(mappedDairyType);
        }
    }
}