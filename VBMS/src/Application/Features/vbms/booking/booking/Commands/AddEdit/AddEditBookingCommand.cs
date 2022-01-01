using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Interfaces.Services;
using VBMS.Domain.Entities.vbms.bookings;
using VBMS.Shared.Wrapper;

namespace VBMS.Application.Features.vbms.booking.booking.Commands.AddEdit
{
    public partial class AddEditBookingCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public string BookingCode { get; set; }
        [Required]
        public int VehicleTypeId { get; set; }
        [Required]
        public int PartnerId { get; set; }
        [Required]
        public string BookingType { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
    }

    internal class AddEditBookingCommandHandler : IRequestHandler<AddEditBookingCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IUploadService _uploadService;
        private readonly IStringLocalizer<AddEditBookingCommandHandler> _localizer;

        public AddEditBookingCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IUploadService uploadService, IStringLocalizer<AddEditBookingCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadService = uploadService;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditBookingCommand command, CancellationToken cancellationToken)
        {
            if (await _unitOfWork.Repository<Booking>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.BookingCode == command.BookingCode, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Booking already exists."]);
            }

            //var uploadRequest = command.UploadRequest;
            //if (uploadRequest != null)
            //{
            //    uploadRequest.FileName = $"P-{command.Barcode}{uploadRequest.Extension}";
            //}

            if (command.Id == 0)
            {
                var booking = _mapper.Map<Booking>(command);
                //if (uploadRequest != null)
                //{
                //    booking.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                //}
                await _unitOfWork.Repository<Booking>().AddAsync(booking);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(booking.Id, _localizer["Booking Saved"]);
            }
            else
            {
                var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(command.Id);
                if (booking != null)
                {
                    booking.BookingCode = command.BookingCode ?? booking.BookingCode;
                    booking.BookingType = command.BookingType ?? booking.BookingType;
                    //if (uploadRequest != null)
                    //{
                    //    booking.ImageDataURL = _uploadService.UploadAsync(uploadRequest);
                    //}
                    booking.StartDate = command.StartDate;
                    booking.EndDate = command.EndDate;
                    booking.VehicleTypeId = command.VehicleTypeId == 0 ? booking.VehicleTypeId : command.VehicleTypeId;
                    booking.PartnerId = command.PartnerId == 0 ? booking.PartnerId : command.PartnerId;
                    await _unitOfWork.Repository<Booking>().UpdateAsync(booking);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(booking.Id, _localizer["Booking Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Booking Not Found!"]);
                }
            }
        }
    }
}