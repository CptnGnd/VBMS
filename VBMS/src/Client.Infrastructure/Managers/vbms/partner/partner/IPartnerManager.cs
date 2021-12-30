using System.Threading.Tasks;
using VBMS.Application.Features.vbms.partner.partner.Commands.AddEdit;
using VBMS.Application.Features.vbms.partner.partner.Queries.GetAllPaged;
using VBMS.Application.Requests.vbms.partner;
using VBMS.Shared.Wrapper;

namespace VBMS.Client.Infrastructure.Managers.vbms.partner.partner
{
    public interface IPartnerManager : IManager
    {
        Task<PaginatedResult<GetAllPagedPartnersResponse>> GetPartnersAsync(GetAllPagedPartnersRequest request);

        Task<IResult<string>> GetPartnerImageAsync(int id);

        Task<IResult<int>> SaveAsync(AddEditPartnerCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}