using System.Threading.Tasks;

namespace VBMS.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<bool> IsVehicleTypeUsed(int vehicleTypeId);
        Task<bool> IsPartnerUsed(int partnerId);
    }
}