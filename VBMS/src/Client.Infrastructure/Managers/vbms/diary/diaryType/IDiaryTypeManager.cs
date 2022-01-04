using System.Collections.Generic;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.diary.diaryType.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.diaryType.Queries.GetAll;
using VBMS.Shared.Wrapper;

namespace VBMS.Client.Infrastructure.Managers.vbms.diary.diaryType
{
    public interface IDiaryTypeManager : IManager
    {
        Task<IResult<List<GetAllDiaryTypesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditDiaryTypeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}