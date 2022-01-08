using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.bookings;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.booking.booking.Queries.GetById
{
    public class GetBookingByIdQuery : IRequest<Result<GetBookingByIdResponse>>
    {
        public int Id { get; set; }
    }

    internal class GetProductTestByIdQueryHandler : IRequestHandler<GetBookingByIdQuery, Result<GetBookingByIdResponse>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;

        public GetProductTestByIdQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<GetBookingByIdResponse>> Handle(GetBookingByIdQuery query, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(query.Id);
            var mappedBooking = _mapper.Map<GetBookingByIdResponse>(booking);
            return await Result<GetBookingByIdResponse>.SuccessAsync(mappedBooking);
        }
    }
}