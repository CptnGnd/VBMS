using System.Threading.Tasks;

namespace VBMS.Application.Interfaces.Repositories
{
    public interface IVehicleTypeAttributeRepository
    {
        Task<bool> IsVehicleTypeUsed(int vehicleTypeId);
    }
}