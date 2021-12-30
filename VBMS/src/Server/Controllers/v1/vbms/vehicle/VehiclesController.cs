using VBMS.Shared.Constants.Permission;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetAllPaged;
using VBMS.Application.Features.vbms.vehicle.vehicle.Commands.AddEdit;
using VBMS.Application.Features.vbms.vehicle.vehicle.Commands.Delete;
using VBMS.Application.Features.vbms.vehicle.vehicle.Queries.Export;
using VBMS.Application.Features.vbms.vehicle.vehicle.Queries.GetVehicleImage;

namespace VBMS.Server.Controllers.v1.vbms.vehicle
{
    public class VehiclesController : BaseApiController<VehiclesController>
    {
        /// <summary>
        /// Get All Vehicles
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Vehicles.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var vehicles = await _mediator.Send(new GetAllVehiclesQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(vehicles);
        }

        /// <summary>
        /// Get a Vehicle Image by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Vehicles.View)]
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetVehicleImageAsync(int id)
        {
            var result = await _mediator.Send(new GetVehicleImageQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Add/Edit a Vehicle
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Vehicles.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditVehicleCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Vehicle
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Vehicles.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteVehicleCommand { Id = id }));
        }

        /// <summary>
        /// Search Vehicles and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Vehicles.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportVehiclesQuery(searchString)));
        }
    }
}