using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.diary.diaryType.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.diaryType.Commands.Delete;
using VBMS.Application.Features.vbms.diary.diaryType.Queries.Export;
using VBMS.Application.Features.vbms.diary.diaryType.Queries.GetAll;
using VBMS.Application.Features.vbms.diary.diaryType.Queries.GetById;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Server.Controllers.v1.vbms.diary
{
    public class DiaryTypesController : BaseApiController<DiaryTypesController>
    {
        /// <summary>
        /// Get All DiaryTypes
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DiaryTypes.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brandTests = await _mediator.Send(new GetAllDiaryTypesQuery());
            return Ok(brandTests);
        }

        /// <summary>
        /// Get a DiaryType By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.DiaryTypes.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brandTest = await _mediator.Send(new GetDiaryTypeByIdQuery() { Id = id });
            return Ok(brandTest);
        }

        /// <summary>
        /// Create/Update a DiaryType
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DiaryTypes.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditDiaryTypeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a DiaryType
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.DiaryTypes.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteDiaryTypeCommand { Id = id }));
        }

        /// <summary>
        /// Search DiaryTypes and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.DiaryTypes.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportDiaryTypesQuery(searchString)));
        }
    }
}