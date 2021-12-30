using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;

namespace VBMS.Infrastructure.Repositories
{
    public class BrandTestRepository : IBrandTestRepository
    {
        private readonly IRepositoryAsync<BrandTest, int> _repository;

        public BrandTestRepository(IRepositoryAsync<BrandTest, int> repository)
        {
            _repository = repository;
        }
    }
}