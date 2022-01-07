using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VBMS.Application.Features.Diarys.Queries.Export;
using VBMS.Application.Features.vbms.diary.diary.Commands.AddEdit;
using VBMS.Application.Features.vbms.diary.diary.Commands.Delete;
using VBMS.Application.Features.vbms.diary.diary.Queries.GetAllPaged;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Server.Controllers.v1.vbms.diary
{
    public class DiarysController : BaseApiController<DiarysController>
    {
        /// <summary>
        /// Get All Diarys
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Diarys.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var productTests = await _mediator.Send(new GetAllDiarysQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(productTests);
        }

        ///// <summary>
        ///// Get a Diary Image by Id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Diarys.View)]
        //[HttpGet("image/{id}")]
        //public async Task<IActionResult> GetDiaryImageAsync(int id)
        //{
        //    var result = await _mediator.Send(new GetDiaryImageQuery(id));
        //    return Ok(result);
        //}

        /// <summary>
        /// Add/Edit a Diary
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Diarys.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditDiaryCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Diary
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Diarys.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteDiaryCommand { Id = id }));
        }

        /// <summary>
        /// Search Diarys and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Diarys.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportDiarysQuery(searchString)));
        }
    }
}