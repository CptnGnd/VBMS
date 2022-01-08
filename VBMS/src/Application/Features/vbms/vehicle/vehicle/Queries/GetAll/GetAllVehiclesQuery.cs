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
using VBMS.Domain.Entities.vbms.bookings;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetAll
{
    public class GetAllVehiclesQuery : IRequest<Result<List<GetAllVehiclesResponse>>>
    {
        public GetAllVehiclesQuery()
        {
        }
    }

    internal class GetAllVehiclesCachedQueryHandler : IRequestHandler<GetAllVehiclesQuery, Result<List<GetAllVehiclesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllVehiclesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllVehiclesResponse>>> Handle(GetAllVehiclesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Vehicle>>> getAllVehicles = () => _unitOfWork.Repository<Vehicle>().GetAllAsync();
            var brandTestList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllVehiclesCacheKey, getAllVehicles);
            var mappedVehicles = _mapper.Map<List<GetAllVehiclesResponse>>(brandTestList);
            return await Result<List<GetAllVehiclesResponse>>.SuccessAsync(mappedVehicles);
        }
    }
}