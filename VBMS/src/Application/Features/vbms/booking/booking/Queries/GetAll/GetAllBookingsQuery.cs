using AutoMapper;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Constants.Application;
using VBMS.Shared.Wrapper;
using LazyCache;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Domain.Entities.vbms.bookings;

namespace VBMS.Application.Features.vbms.booking.booking.Queries.GetAll
{
    public class GetAllBookingsQuery : IRequest<Result<List<GetAllBookingsResponse>>>
    {
        public GetAllBookingsQuery()
        {
        }
    }

    internal class GetAllBookingsCachedQueryHandler : IRequestHandler<GetAllBookingsQuery, Result<List<GetAllBookingsResponse>>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAppCache _cache;

        public GetAllBookingsCachedQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IAppCache cache)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _cache = cache;
        }

        public async Task<Result<List<GetAllBookingsResponse>>> Handle(GetAllBookingsQuery request, CancellationToken cancellationToken)
        {
            Func<Task<List<Booking>>> getAllBookings = () => _unitOfWork.Repository<Booking>().GetAllAsync();
            var brandTestList = await _cache.GetOrAddAsync(ApplicationConstants.Cache.GetAllBookingsCacheKey, getAllBookings);
            var mappedBookings = _mapper.Map<List<GetAllBookingsResponse>>(brandTestList);
            return await Result<List<GetAllBookingsResponse>>.SuccessAsync(mappedBookings);
        }
    }
}