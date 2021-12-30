using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.partner.partnerType.Commands.AddEdit;
using VBMS.Application.Features.vbms.partner.partnerType.Commands.Delete;
using VBMS.Application.Features.vbms.partner.partnerType.Queries.Export;
using VBMS.Application.Features.vbms.partner.partnerType.Queries.GetAll;
using VBMS.Application.Features.vbms.partner.partnerType.Queries.GetById;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Server.Controllers.v1.vbms.partners
{
    public class PartnerTypesController : BaseApiController<PartnerTypesController>
    {
        /// <summary>
        /// Get All PartnerTypes
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.PartnerTypes.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var PartnerTypes = await _mediator.Send(new GetAllPartnerTypesQuery());
            return Ok(PartnerTypes);
        }

        /// <summary>
        /// Get a PartnerType By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.PartnerTypes.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var PartnerType = await _mediator.Send(new GetPartnerTypeByIdQuery() { Id = id });
            return Ok(PartnerType);
        }

        /// <summary>
        /// Create/Update a PartnerType
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.PartnerTypes.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPartnerTypeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a PartnerType
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.PartnerTypes.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeletePartnerTypeCommand { Id = id }));
        }

        /// <summary>
        /// Search PartnerTypes and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.PartnerTypes.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportPartnerTypesQuery(searchString)));
        }
    }
}