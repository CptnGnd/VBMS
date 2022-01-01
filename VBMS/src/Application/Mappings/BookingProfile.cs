using AutoMapper;
using VBMS.Application.Features.vbms.booking.booking.Commands.AddEdit;
using VBMS.Domain.Entities.vbms.bookings;

namespace VBMS.Application.Mappings
{
    public class BookingProfile : Profile
    {
        public BookingProfile()
        {
            CreateMap<AddEditBookingCommand, Booking>().ReverseMap();
        }
    }
}