using VBMS.Application.Features.ProductTests.Commands.AddEdit;
using VBMS.Application.Features.ProductTests.Queries.GetAllPaged;
using VBMS.Application.Requests.Catalog;
using VBMS.Shared.Wrapper;
using System.Threading.Tasks;

namespace VBMS.Client.Infrastructure.Managers.Catalog.ProductTest
{
    public interface IProductTestManager : IManager
    {
        Task<PaginatedResult<GetAllPagedProductTestsResponse>> GetProductTestsAsync(GetAllPagedProductTestsRequest request);

        Task<IResult<string>> GetProductTestImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditProductTestCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}