using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.vehicle.vehicleTypeAttributes.Commands.Delete;
using VBMS.Application.Features.vbms.vehicle.vehicleTypeAttributes.Queries.Export;
using VBMS.Application.Features.vbms.vehicle.vehicleTypeAttributes.Queries.GetAllPaged;
using VBMS.Application.Features.VehicleTypeAttributes.Commands.AddEdit;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Server.Controllers.v1.vbms.vehicle
{
    public class VehicleTypeAttributesController : BaseApiController<VehicleTypeAttributesController>
    {
        /// <summary>
        /// Get All VehicleTypeAttributes
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.VehicleTypeAttributes.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var productTests = await _mediator.Send(new GetAllVehicleTypeAttributesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(productTests);
        }

        ///// <summary>
        ///// Get a VehicleTypeAttribute Image by Id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.VehicleTypeAttributes.View)]
        //[HttpGet("image/{id}")]
        //public async Task<IActionResult> GetVehicleTypeAttributeImageAsync(int id)
        //{
        //    var result = await _mediator.Send(new GetVehicleTypeAttributeImageQuery(id));
        //    return Ok(result);
        //}

        /// <summary>
        /// Add/Edit a VehicleTypeAttribute
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.VehicleTypeAttributes.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditVehicleTypeAttributeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a VehicleTypeAttribute
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.VehicleTypeAttributes.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteVehicleTypeAttributeCommand { Id = id }));
        }

        /// <summary>
        /// Search VehicleTypeAttributes and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.VehicleTypeAttributes.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportVehicleTypeAttributesQuery(searchString)));
        }
    }
}