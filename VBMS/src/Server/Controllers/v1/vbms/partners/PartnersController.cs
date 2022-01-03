using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.partner.partner.Commands.AddEdit;
using VBMS.Application.Features.vbms.partner.partner.Commands.Delete;
using VBMS.Application.Features.vbms.partner.partner.Queries.Export;
using VBMS.Application.Features.vbms.partner.partner.Queries.GetAll;
using VBMS.Application.Features.vbms.partner.partner.Queries.GetAllPaged;
using VBMS.Application.Features.vbms.partner.partner.Queries.GetProductImage;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Server.Controllers.v1.vbms.partners
{
    public class PartnersController : BaseApiController<PartnersController>
    {
        /// <summary>
        /// Get All Partners
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Partners.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var partners = await _mediator.Send(new GetAllPartnersQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(partners);
        }

        /// <summary>
        /// Get All PartnerTypes
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Partners.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var PartnerTypes = await _mediator.Send(new GetAllAllPartnersQuery());
            return Ok(PartnerTypes);
        }

        /// <summary>
        /// Get a Partner Image by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Partners.View)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetPartnerImageAsync(int id)
        {
            var result = await _mediator.Send(new GetPartnerImageQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Add/Edit a Partner
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Partners.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditPartnerCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Partner
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Partners.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeletePartnerCommand { Id = id }));
        }

        /// <summary>
        /// Search Partners and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Partners.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportPartnersQuery(searchString)));
        }
    }
}