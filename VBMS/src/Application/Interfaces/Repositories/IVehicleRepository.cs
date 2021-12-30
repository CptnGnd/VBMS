using System.Threading.Tasks;

namespace VBMS.Application.Interfaces.Repositories
{
    public interface IVehicleRepository
    {
        Task<bool> IsVehicleTypeUsed(int brandTestId);
    }
}