using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Domain.Entities.vbms.partners;

namespace VBMS.Infrastructure.Repositories
{
    public class PartnerTypeRepository : IPartnerTypeRepository
    {
        private readonly IRepositoryAsync<PartnerType, int> _repository;

        public PartnerTypeRepository(IRepositoryAsync<PartnerType, int> repository)
        {
            _repository = repository;
        }
    }
}