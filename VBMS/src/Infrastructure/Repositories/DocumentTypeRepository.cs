using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Misc;

namespace VBMS.Infrastructure.Repositories
{
    public class DocumentTypeRepository : IDocumentTypeRepository
    {
        private readonly IRepositoryAsync<DocumentType, int> _repository;

        public DocumentTypeRepository(IRepositoryAsync<DocumentType, int> repository)
        {
            _repository = repository;
        }
    }
}