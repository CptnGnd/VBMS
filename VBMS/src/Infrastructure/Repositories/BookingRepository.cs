using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.bookings;

namespace VBMS.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IRepositoryAsync<Booking, int> _repository;

        public BookingRepository(IRepositoryAsync<Booking, int> repository)
        {
            _repository = repository;
        }

        public async Task<bool> IsPartnerUsed(int partnerId)
        {
            return await _repository.Entities.AnyAsync(b => b.PartnerId == partnerId);
        }

        public async Task<bool> IsVehicleTypeUsed(int vehicleTypeId)
        {
            return await _repository.Entities.AnyAsync(b => b.VehicleTypeId == vehicleTypeId);
        }
    }
}