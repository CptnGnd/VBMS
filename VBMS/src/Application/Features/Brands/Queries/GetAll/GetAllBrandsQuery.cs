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

namespace VBMS.Application.Features.BrandTests.Queries.GetAll
{
    public class GetAllBrandTestsQuery : IRequest<Result<List<GetAllVehicleTypessResponse>>>
    {
        public GetAllBrandTestsQuery()
        {
        }
    }

    internal class GetAllBrandTestsCachedQueryHandler : IRequestHandler<GetAllBrandTestsQuery, Result<List<GetAllVehicleTypessResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllBrandTestsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllVehicleTypessResponse>>> Handle(GetAllBrandTestsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<BrandTest>>> getAllBrandTests = () => _unitOfWork.Repository<BrandTest>().GetAllAsync();
            var brandTestList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllBrandTestsCacheKey, getAllBrandTests);
            var mappedBrandTests = _mapper.Map<List<GetAllVehicleTypessResponse>>(brandTestList);
            return await Result<List<GetAllVehicleTypessResponse>>.SuccessAsync(mappedBrandTests);
        }
    }
}