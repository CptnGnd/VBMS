using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.diary;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.diary.diaryType.Queries.GetById
{
    public class GetDiaryTypeByIdQuery : IRequest<Result<GetDiaryTypeByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetDiaryTypeByIdQueryHandler : IRequestHandler<GetDiaryTypeByIdQuery, Result<GetDiaryTypeByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetDiaryTypeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetDiaryTypeByIdResponse>> Handle(GetDiaryTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var diaryType = await _unitOfWork.Repository<DiaryType>().GetByIdAsync(query.Id);
            var mappedDiaryType = _mapper.Map<GetDiaryTypeByIdResponse>(diaryType);
            return await Result<GetDiaryTypeByIdResponse>.SuccessAsync(mappedDiaryType);
        }
    }
}