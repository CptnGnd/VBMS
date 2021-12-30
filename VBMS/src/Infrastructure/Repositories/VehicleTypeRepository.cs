using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Infrastructure.Repositories
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        private readonly IRepositoryAsync<VehicleType, int> _repository;

        public VehicleTypeRepository(IRepositoryAsync<VehicleType, int> repository)
        {
            _repository = repository;
        }
    }
}