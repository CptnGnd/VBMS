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

namespace VBMS.Application.Features.vbms.partner.partner.Queries.GetAll
{
    public class GetAllAllPartnersQuery : IRequest<Result<List<GetAllPartnersResponse>>>
    {
        public GetAllAllPartnersQuery()
        {
        }
    }

    internal class GetAllPartnersCachedQueryHandler : IRequestHandler<GetAllAllPartnersQuery, Result<List<GetAllPartnersResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllPartnersCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllPartnersResponse>>> Handle(GetAllAllPartnersQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Partner>>> getAllPartners = () => _unitOfWork.Repository<Partner>().GetAllAsync();
            var partnerList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllPartnersCacheKey, getAllPartners);
            var mappedPartners = _mapper.Map<List<GetAllPartnersResponse>>(partnerList);
            return await Result<List<GetAllPartnersResponse>>.SuccessAsync(mappedPartners);
        }
    }
}