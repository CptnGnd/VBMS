using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using VBMS.Application.Features.vbms.booking.booking.Commands.AddEdit;
using VBMS.Application.Features.vbms.booking.booking.Commands.Delete;
using VBMS.Application.Features.vbms.booking.booking.Queries.Export;
using VBMS.Application.Features.vbms.booking.booking.Queries.GetAllPaged;
using VBMS.Application.Features.vbms.booking.booking.Queries.GetById;
using VBMS.Shared.Constants.Permission;

namespace VBMS.Server.Controllers.v1.Catalog
{
    public class BookingsController : BaseApiController<BookingsController>
    {
        /// <summary>
        /// Get All Bookings
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchString"></param>
        /// <param name="orderBy"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Bookings.View)]
        [HttpGet]
        public async Task<IActionResult> GetAll(int pageNumber, int pageSize, string searchString, string orderBy = null)
        {
            var bookings = await _mediator.Send(new GetAllBookingsQuery(pageNumber, pageSize, searchString, orderBy));
            return Ok(bookings);
        }

        /// <summary>
        /// Get a PartnerType By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 Ok</returns>
        [Authorize(Policy = Permissions.Bookings.View)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var Booking = await _mediator.Send(new GetBookingByIdQuery() { Id = id });
            return Ok(Booking);
        }
        ///// <summary>
        ///// Get a Booking Image by Id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns>Status 200 OK</returns>
        //[Authorize(Policy = Permissions.Bookings.View)]
        //[HttpGet("image/{id}")]
        //public async Task<IActionResult> GetBookingImageAsync(int id)
        //{
        //    var result = await _mediator.Send(new GetBookingImageQuery(id));
        //    return Ok(result);
        //}

        /// <summary>
        /// Add/Edit a Booking
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Bookings.Create)]
        [HttpPost]
        public async Task<IActionResult> Post(AddEditBookingCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        /// <summary>
        /// Delete a Booking
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Status 200 OK response</returns>
        [Authorize(Policy = Permissions.Bookings.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return Ok(await _mediator.Send(new DeleteBookingCommand { Id = id }));
        }

        /// <summary>
        /// Search Bookings and Export to Excel
        /// </summary>
        /// <param name="searchString"></param>
        /// <returns>Status 200 OK</returns>
        [Authorize(Policy = Permissions.Bookings.Export)]
        [HttpGet("export")]
        public async Task<IActionResult> Export(string searchString = "")
        {
            return Ok(await _mediator.Send(new ExportBookingsQuery(searchString)));
        }
    }
}