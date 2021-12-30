using VBMS.Application.Features.BrandTests.Queries.GetAll;
using VBMS.Application.Features.BrandTests.Queries.GetById;
using VBMS.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VBMS.Application.Features.BrandTests.Commands.AddEdit;
using VBMS.Application.Features.BrandTests.Commands.Delete;
using VBMS.Application.Features.BrandTests.Queries.Export;

namespace VBMS.Server.Controllers.v1.Catalog
{
    public class BrandTestsController : BaseApiController<BrandTestsController>
    {
        /// <summary>
        /// Get All BrandTests
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BrandTests.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var brandTests = await _mediator.Send(new GetAllBrandTestsQuery());
            return Ok(brandTests);
        }

        /// <summary>
        /// Get a BrandTest By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.BrandTests.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var brandTest = await _mediator.Send(new GetBrandTestByIdQuery() { Id = id });
            return Ok(brandTest);
        }

        /// <summary>
        /// Create/Update a BrandTest
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BrandTests.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditBrandTestCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a BrandTest
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.BrandTests.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteBrandTestCommand { Id = id }));
        }

        /// <summary>
        /// Search BrandTests and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.BrandTests.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportBrandTestsQuery(searchString)));
        }
    }
}