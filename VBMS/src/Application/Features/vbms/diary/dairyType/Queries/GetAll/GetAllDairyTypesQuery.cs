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

namespace VBMS.Application.Features.vbms.diary.dairyType.Queries.GetAll
{
    public class GetAllDairyTypesQuery : IRequest<Result<List<GetAllDairyTypesResponse>>>
    {
        public GetAllDairyTypesQuery()
        {
        }
    }

    internal class GetAllDairyTypesCachedQueryHandler : IRequestHandler<GetAllDairyTypesQuery, Result<List<GetAllDairyTypesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllDairyTypesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllDairyTypesResponse>>> Handle(GetAllDairyTypesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<DairyType>>> getAllDairyTypes = () => _unitOfWork.Repository<DairyType>().GetAllAsync();
            var dairyTypeList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllDairyTypesCacheKey, getAllDairyTypes);
            var mappedDairyTypes = _mapper.Map<List<GetAllDairyTypesResponse>>(dairyTypeList);
            return await Result<List<GetAllDairyTypesResponse>>.SuccessAsync(mappedDairyTypes);
        }
    }
}