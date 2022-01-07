using System.Threading.Tasks;
using VBMS.Application.Features.vbms.diary.diary.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.diary.Queries.GetAllPaged;
using VBMS.Application.Requests.vbms.diary;
using VBMS.Shared.Wrapper;

namespace VBMS.Client.Infrastructure.Managers.Catalog.Diary
{
    public interface IDiaryManager : IManager
    {
        Task<PaginatedResult<GetAllPagedDiarysResponse>> GetDiarysAsync(GetAllPagedDiarysRequest request);

      //  Task<IResult<string>> GetDiaryImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditDiaryCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}