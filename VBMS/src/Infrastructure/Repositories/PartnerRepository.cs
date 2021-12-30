using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.partners;

namespace VBMS.Infrastructure.Repositories
{
    public class PartnerRepository : IPartnerRepository
    {
        private readonly IRepositoryAsync<Partner, int> _repository;

        public PartnerRepository(IRepositoryAsync<Partner, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsPartnerTypeUsed(int partnerTypeId)
        {
            return await _repository.Entities.AnyAsync(b => b.PartnerTypeId == partnerTypeId);
        }
    }
}