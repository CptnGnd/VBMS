using VBMS.Application.Features.ProductTests.Commands.AddEdit;
using VBMS.Application.Features.ProductTests.Commands.Delete;
using VBMS.Application.Features.ProductTests.Queries.Export;
using VBMS.Application.Features.ProductTests.Queries.GetAllPaged;
using VBMS.Application.Features.ProductTests.Queries.GetProductTestImage;
using VBMS.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace VBMS.Server.Controllers.v1.Catalog
{
    public class ProductTestsController : BaseApiController<ProductTestsController>
    {
        /// <summary>
        /// Get All ProductTests
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductTests.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var productTests = await _mediator.Send(new GetAllProductTestsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(productTests);
        }

        /// <summary>
        /// Get a ProductTest Image by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductTests.View)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetProductTestImageAsync(int id)
        {
            var result = await _mediator.Send(new GetProductTestImageQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Add/Edit a ProductTest
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductTests.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditProductTestCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a ProductTest
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.ProductTests.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteProductTestCommand { Id = id }));
        }

        /// <summary>
        /// Search ProductTests and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.ProductTests.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportProductTestsQuery(searchString)));
        }
    }
}