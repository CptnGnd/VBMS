using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace VBMS.Infrastructure.Repositories
{
    public class ProductTestRepository : IProductTestRepository
    {
        private readonly IRepositoryAsync<ProductTest, int> _repository;

        public ProductTestRepository(IRepositoryAsync<ProductTest, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsBrandTestUsed(int brandTestId)
        {
            return await _repository.Entities.AnyAsync(b => b.BrandTestId == brandTestId);
        }
    }
}