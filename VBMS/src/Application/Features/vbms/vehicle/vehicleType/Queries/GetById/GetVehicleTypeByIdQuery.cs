using AutoMapper;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.GetById
{
    public class GetVehicleTypeByIdQuery : IRequest<Result<GetVehicleTypeByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetVehicleTypeByIdQueryHandler : IRequestHandler<GetVehicleTypeByIdQuery, Result<GetVehicleTypeByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetVehicleTypeByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetVehicleTypeByIdResponse>> Handle(GetVehicleTypeByIdQuery query, CancellationToken cancellationToken)
        {
            var brandTest = await _unitOfWork.Repository<VehicleType>().GetByIdAsync(query.Id);
            var mappedVehicleType = _mapper.Map<GetVehicleTypeByIdResponse>(brandTest);
            return await Result<GetVehicleTypeByIdResponse>.SuccessAsync(mappedVehicleType);
        }
    }
}