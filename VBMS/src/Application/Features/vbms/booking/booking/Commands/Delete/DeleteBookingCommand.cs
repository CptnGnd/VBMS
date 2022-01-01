using MediatR;
using Microsoft.Extensions.Localization;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Domain.Entities.vbms.bookings;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.booking.booking.Commands.Delete
{
    public class DeleteBookingCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
    }

    internal class DeleteBookingCommandHandler : IRequestHandler<DeleteBookingCommand, Result<int>>
    {
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<DeleteBookingCommandHandler> _localizer;

        public DeleteBookingCommandHandler(IUnitOfWork<int> unitOfWork, IStringLocalizer<DeleteBookingCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(DeleteBookingCommand command, CancellationToken cancellationToken)
        {
            var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(command.Id);
            if (booking != null)
            {
                await _unitOfWork.Repository<Booking>().DeleteAsync(booking);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(booking.Id, _localizer["Booking Deleted"]);
            }
            else
            {
                return await Result<int>.FailAsync(_localizer["Booking Not Found!"]);
            }
        }
    }
}