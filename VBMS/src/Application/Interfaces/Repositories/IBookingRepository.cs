using System.Threading.Tasks;

namespace VBMS.Application.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<bool> IsVehbicleTypeUsed(int vehicleTypeId);
        Task<bool> IsPartnerUsed(int partnerId);
    }
}