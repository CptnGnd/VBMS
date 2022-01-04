using AutoMapper;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Constants.Application;
using VBMS.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Domain.Entities.vbms.diary;

namespace VBMS.Application.Features.vbms.diary.diaryType.Queries.GetAll
{
    public class GetAllDiaryTypesQuery : IRequest<Result<List<GetAllDiaryTypesResponse>>>
    {
        public GetAllDiaryTypesQuery()
        {
        }
    }

    internal class GetAllDiaryTypesCachedQueryHandler : IRequestHandler<GetAllDiaryTypesQuery, Result<List<GetAllDiaryTypesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllDiaryTypesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllDiaryTypesResponse>>> Handle(GetAllDiaryTypesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<DiaryType>>> getAllDiaryTypes = () => _unitOfWork.Repository<DiaryType>().GetAllAsync();
            var diaryTypeList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllDiaryTypesCacheKey, getAllDiaryTypes);
            var mappedDiaryTypes = _mapper.Map<List<GetAllDiaryTypesResponse>>(diaryTypeList);
            return await Result<List<GetAllDiaryTypesResponse>>.SuccessAsync(mappedDiaryTypes);
        }
    }
}