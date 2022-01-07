using VBMS.Application.Specifications.Base;
using VBMS.Domain.Entities.vbms.diary;

namespace VBMS.Application.Specifications.Catalog
{
    public class DiaryFilterSpecification : HeroSpecification<Diary>
    {
        public DiaryFilterSpecification(string searchString)
        {
            Includes.Add(a => a.Vehicle);
            Includes.Add(a => a.DiaryType);
            Includes.Add(a => a.Booking);
            if (!string.IsNullOrEmpty(searchString))
            {
                Criteria = p => p.StartDateTime != null && (p.Vehicle.Rego.Contains(searchString) || p.DiaryType.Name.Contains(searchString) || p.Booking.BookingCode.Contains(searchString) || p.Vehicle.VehicleType.Name.Contains(searchString));
            }
            else
            {
                Criteria = p => p.StartDateTime != null;
            }
        }
    }
}