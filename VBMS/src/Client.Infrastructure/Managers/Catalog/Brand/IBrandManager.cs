using VBMS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using VBMS.Application.Features.BrandTests.Commands.AddEdit;
using VBMS.Application.Features.vbms.booking.booking.Queries.GetAll;
using VBMS.Application.Features.BrandTests.Queries.GetAll;

namespace VBMS.Client.Infrastructure.Managers.Catalog.BrandTest
{
    public interface IBrandTestManager : IManager
    {
        Task<IResult<List<GetAllBrandTestsResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditBrandTestCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}