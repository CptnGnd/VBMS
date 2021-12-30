using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Infrastructure.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly IRepositoryAsync<Vehicle, int> _repository;

        public VehicleRepository(IRepositoryAsync<Vehicle, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsVehicleTypeUsed(int vehicleTypeId)
        {
            return await _repository.Entities.AnyAsync(b => b.VehicleTypeId == vehicleTypeId);
        }
    }
}