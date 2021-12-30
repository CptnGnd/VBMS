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
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.GetAll
{
    public class GetAllVehicleTypesQuery : IRequest<Result<List<GetAllVehicleTypesResponse>>>
    {
        public GetAllVehicleTypesQuery()
        {
        }
    }

    internal class GetAllVehicleTypesCachedQueryHandler : IRequestHandler<GetAllVehicleTypesQuery, Result<List<GetAllVehicleTypesResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllVehicleTypesCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllVehicleTypesResponse>>> Handle(GetAllVehicleTypesQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<VehicleType>>> getAllVehicleTypes = () => _unitOfWork.Repository<VehicleType>().GetAllAsync();
            var vehicleTypeList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllVehicleTypesCacheKey, getAllVehicleTypes);
            var mappedVehicleTypes = _mapper.Map<List<GetAllVehicleTypesResponse>>(vehicleTypeList);
            return await Result<List<GetAllVehicleTypesResponse>>.SuccessAsync(mappedVehicleTypes);
        }
    }
}