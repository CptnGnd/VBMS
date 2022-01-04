using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.diary.dairyType.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.dairyType.Commands.Delete;
using VBMS.Application.Features.vbms.diary.dairyType.Queries.Export;
using VBMS.Application.Features.vbms.diary.dairyType.Queries.GetAll;
using VBMS.Application.Features.vbms.diary.dairyType.Queries.GetById;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Server.Controllers.v1.vbms.diary
{
    public class DairyTypesController : BaseApiController<DairyTypesController>
    {
        /// <summary>
        /// Get All DairyTypes
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DairyTypes.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brandTests = await _mediator.Send(new GetAllDairyTypesQuery());
            return Ok(brandTests);
        }

        /// <summary>
        /// Get a DairyType By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.DairyTypes.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brandTest = await _mediator.Send(new GetDairyTypeByIdQuery() { Id = id });
            return Ok(brandTest);
        }

        /// <summary>
        /// Create/Update a DairyType
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DairyTypes.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditDairyTypeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a DairyType
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DairyTypes.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteDairyTypeCommand { Id = id }));
        }

        /// <summary>
        /// Search DairyTypes and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.DairyTypes.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportDairyTypesQuery(searchString)));
        }
    }
}