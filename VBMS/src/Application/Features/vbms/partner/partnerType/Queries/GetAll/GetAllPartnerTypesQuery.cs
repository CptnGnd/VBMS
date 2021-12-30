using AutoMapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.partners;
using VBMS.Shared.Constants.Application;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.partner.partnerType.Queries.GetAll
{
    public class GetAllPartnerTypesQuery : IRequest<Result<List<GetAllPartnerTypesResponse>>>
    {
        public GetAllPartnerTypesQuery()
        {
        }
    }

    internal class GetAllPartnerTypesCachedQueryHandler : IRequestHandler<GetAllPartnerTypesQuery, Result<List<GetAllPartnerTypesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllPartnerTypesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllPartnerTypesResponse>>> Handle(GetAllPartnerTypesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<PartnerType>>> getAllPartnerTypes = () => _unitOfWork.Repository<PartnerType>().GetAllAsync();
            var partnerTypeList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllPartnerTypesCacheKey, getAllPartnerTypes);
            var mappedPartnerTypes = _mapper.Map<List<GetAllPartnerTypesResponse>>(partnerTypeList);
            return await Result<List<GetAllPartnerTypesResponse>>.SuccessAsync(mappedPartnerTypes);
        }
    }
}