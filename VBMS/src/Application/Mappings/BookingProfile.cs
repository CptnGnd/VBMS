using AutoMapper;
using VBMS.Application.Features.vbms.booking.booking.Commands.AddEdit;
using VBMS.Application.Features.vbms.booking.booking.Queries.GetById;
using VBMS.Domain.Entities.vbms.bookings;

namespace VBMS.Application.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<AddEditBookingCommand, Booking>().ReverseMap();
            CreateMap<GetBookingByIdResponse, Booking>().ReverseMap();
        }
    }
}