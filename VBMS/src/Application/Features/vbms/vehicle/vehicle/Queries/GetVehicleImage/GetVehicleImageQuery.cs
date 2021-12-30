using VBMS.Application.Interfaces.Repositories;
using VBMS.Shared.Wrapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetVehicleImage
{
    public class GetVehicleImageQuery : IRequest<Result<string>>
    {
        public int Id { get; set; }

        public GetVehicleImageQuery(int productTestId)
        {
            Id = productTestId;
        }
    }

    internal class GetVehicleImageQueryHandler : IRequestHandler<GetVehicleImageQuery, Result<string>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;

        public GetVehicleImageQueryHandler(IUnitOfWork<int> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<string>> Handle(GetVehicleImageQuery request, CancellationToken cancellationToken)
        {
            var data = await _unitOfWork.Repository<Vehicle>().Entities.Where(p => p.Id == request.Id).Select(a => a.ImageDataURL).FirstOrDefaultAsync(cancellationToken);
            return await Result<string>.SuccessAsync(data: data);
        }
    }
}