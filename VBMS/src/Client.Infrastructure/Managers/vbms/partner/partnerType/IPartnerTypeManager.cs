using System.Collections.Generic;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.partner.partnerType.Commands.AddEdit;
using VBMS.Application.Features.vbms.partner.partnerType.Queries.GetAll;
using VBMS.Shared.Wrapper;

namespace VBMS.Client.Infrastructure.Managers.vbms.partner.partnerType
{
    public interface IPartnerTypeManager : IManager
    {
        Task<IResult<List<GetAllPartnerTypesResponse>>> GetAllAsync();

        Task<IResult<int>> SaveAsync(AddEditPartnerTypeCommand request);

        Task<IResult<int>> DeleteAsync(int id);

        Task<IResult<string>> ExportToExcelAsync(string searchString = "");
    }
}