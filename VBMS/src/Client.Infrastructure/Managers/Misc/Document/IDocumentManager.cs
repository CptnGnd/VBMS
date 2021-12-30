using VBMS.Application.Features.Documents.Commands.AddEdit;
using VBMS.Application.Features.Documents.Queries.GetAll;
using VBMS.Application.Requests.Documents;
using VBMS.Shared.Wrapper;
using System.Threading.Tasks;
using VBMS.Application.Features.Documents.Queries.GetById;

namespace VBMS.Client.Infrastructure.Managers.Misc.Document
{
    public interface IDocumentManager : IManager
    {
        Task<PaginatedResult<GetAllDocumentsResponse>> GetAllAsync(GetAllPagedDocumentsRequest request);

        Task<IResult<GetDocumentByIdResponse>> GetByIdAsync(GetDocumentByIdQuery request);

        Task<IResult<int>> SaveAsync(AddEditDocumentCommand request);

        Task<IResult<int>> DeleteAsync(int id);
    }
}