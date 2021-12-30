using System.Threading.Tasks;

namespace VBMS.Application.Interfaces.Repositories
{
    public interface IPartnerRepository
    {
        Task<bool> IsPartnerTypeUsed(int brandTestId);
    }
}