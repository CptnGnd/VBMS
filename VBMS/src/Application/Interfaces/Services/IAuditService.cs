using VBMS.Application.Responses.Audit;
using VBMS.Shared.Wrapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace VBMS.Application.Interfaces.Services
{
    public interface IAuditService
    {
        Task<IResult<IEnumerable<AuditResponse>>> GetCurrentUserTrailsAsync(string userId);

        Task<IResult<string>> ExportToExcelAsync(string userId, string searchString = "", bool searchInOldValues = false, bool searchInNewValues = false);
    }
}