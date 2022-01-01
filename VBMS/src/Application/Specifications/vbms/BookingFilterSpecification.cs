using VBMS.Application.Specifications.Base;
using VBMS.Domain.Entities.vbms.bookings;

namespace VBMS.Application.Specifications.vbms
{
    public class BookingFilterSpecification : HeroSpecification<Booking>
    {
        public BookingFilterSpecification(string searchString)
        {
            Includes.Add(a => a.VehicleType);
            Includes.Add(a => a.Partner);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.BookingCode != null && (p.BookingCode.Contains(searchString) || p.Partner.Name.Contains(searchString) || p.Partner.ShortName.Contains(searchString) || p.VehicleType.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.BookingCode != null;
            }
        }
    }
}