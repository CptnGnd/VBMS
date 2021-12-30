using System.Threading.Tasks;

namespace VBMS.Application.Interfaces.Repositories
{
    public interface IProductTestRepository
    {
        Task<bool> IsBrandTestUsed(int brandTestId);
    }
}