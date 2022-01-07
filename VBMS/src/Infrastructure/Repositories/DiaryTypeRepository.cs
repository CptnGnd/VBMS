using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.diary;

namespace VBMS.Infrastructure.Repositories
{
    public class DiaryTypeRepository : IDiaryTypeRepository
    {
        private readonly IRepositoryAsync<DiaryType, int> _repository;

        public DiaryTypeRepository(IRepositoryAsync<DiaryType, int> repository)
        {
            _repository = repository;
        }
    }
}