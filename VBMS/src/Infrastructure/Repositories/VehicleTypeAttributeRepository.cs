using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.vehicles;

namespace VBMS.Infrastructure.Repositories
{
    public class VehicleTypeAttributeRepository : IVehicleTypeAttributeRepository
    {
        private readonly IRepositoryAsync<VehicleTypeAttribute, int> _repository;

        public VehicleTypeAttributeRepository(IRepositoryAsync<VehicleTypeAttribute, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsVehicleTypeUsed(int vehicleTypeId)
        {
            return await _repository.Entities.AnyAsync(b => b.VehicleTypeId == vehicleTypeId);
        }
    }
}