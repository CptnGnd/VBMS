using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Commands.AddEdit;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Commands.Delete;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.Export;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.GetAll;
using VBMS.Application.Features.vbms.vehicle.vehicleType.Queries.GetById;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Server.Controllers.v1.vbms.vehicle
{
    public class VehicleTypesController : BaseApiController<VehicleTypesController>
    {
        /// <summary>
        /// Get All VehicleTypes
        /// </summary>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.VehicleTypes.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var vehicleTypes = await _mediator.Send(new GetAllVehicleTypesQuery());
            return Ok(vehicleTypes);
        }

        /// <summary>
        /// Get a VehicleType By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.VehicleTypes.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var vehicleType = await _mediator.Send(new GetVehicleTypeByIdQuery() { Id = id });
            return Ok(vehicleType);
        }

        /// <summary>
        /// Create/Update a VehicleType
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.VehicleTypes.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditVehicleTypeCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a VehicleType
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.VehicleTypes.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteVehicleTypeCommand { Id = id }));
        }

        /// <summary>
        /// Search VehicleTypes and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns></returns>
        [Authorize(Policy = Permissions.VehicleTypes.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportVehicleTypesQuery(searchString)));
        }
    }
}