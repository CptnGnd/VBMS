using System.ComponentModel.DataAnnotations;
using AutoMapper;
using VBMS.Application.Interfaces.Repositories;
using VBMS.Application.Interfaces.Services;
using VBMS.Application.Requests;
using VBMS.Domain.Entities.Catalog;
using VBMS.Shared.Wrapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using VBMS.Domain.Entities.vbms.diary;
using System;
using VBMS.Application.Features.vbms.booking.booking.Queries.GetById;
using VBMS.Domain.Entities.vbms.bookings;

namespace VBMS.Application.Features.vbms.diary.diary.Commands.AddEdit
{
    public partial class AddEditDiaryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        public int? BookingId { get; set; }
        [Required]
        public int VehicleId { get; set; }
        [Required]
        public int DiaryTypeId { get; set; }
    }

    internal class AddEditDiaryCommandHandler : IRequestHandler<AddEditDiaryCommand, Result<int>>
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork<int> _unitOfWork;
        private readonly IStringLocalizer<AddEditDiaryCommandHandler> _localizer;

        public AddEditDiaryCommandHandler(IUnitOfWork<int> unitOfWork, IMapper mapper, IStringLocalizer<AddEditDiaryCommandHandler> localizer)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task<Result<int>> Handle(AddEditDiaryCommand command, CancellationToken cancellationToken)
        {
            if (command.BookingId != null)
            {
                var booking = await _unitOfWork.Repository<Booking>().GetByIdAsync(command.BookingId==null?0:(int)command.BookingId);
                if (booking != null)
                {
                    command.StartDateTime = booking.StartDate;
                    command.EndDateTime = booking.EndDate;
                }
            }
            var involvedDiares = await _unitOfWork.Repository<Diary>().Entities.Where(p => p.VehicleId == command.VehicleId && p.StartDateTime <= command.EndDateTime
                           && p.EndDateTime >= command.StartDateTime).ToListAsync();
            foreach (var involvedDiary in involvedDiares) 
            {
                if (involvedDiary.StartDateTime < command.StartDateTime)
                {
                    if (involvedDiary.Id != command.Id)
                    {
                        if (involvedDiary.EndDateTime > command.EndDateTime)
                        {
                            AddEditDiaryCommand endDiary = new AddEditDiaryCommand
                            {
                                StartDateTime = command.EndDateTime.AddSeconds(1),
                                EndDateTime = involvedDiary.EndDateTime,
                                VehicleId = involvedDiary.VehicleId,
                                DiaryTypeId = involvedDiary.DiaryTypeId,
                                BookingId = null,
                                Id = 0
                            };
                            var thisdiary = _mapper.Map<Diary>(endDiary);
                            await _unitOfWork.Repository<Diary>().AddAsync(thisdiary);
                        }
                        involvedDiary.EndDateTime = command.StartDateTime.AddSeconds(-1);
                        involvedDiary.BookingId = null;
                    }
                    else
                    {
                        var k = _unitOfWork.Repository<Diary>().Entities
                                           .Where(p => p.VehicleId == command.VehicleId && p.EndDateTime == command.StartDateTime.AddSeconds(-1));
                    }
                    await _unitOfWork.Repository<Diary>().UpdateAsync(involvedDiary);
                }
            }
            //if (await _unitOfWork.Repository<Diary>().Entities.Where(p => p.Id != command.Id)
            //    .AnyAsync(p => p.VehicleId == command.VehicleId && p.StartDateTime <= command.EndDateTime && p.EndDateTime >= command.StartDateTime, cancellationToken))
            //{
            //    return await Result<int>.FailAsync(_localizer["Diary Redcords may not Overlap"]);
            //}


            if (command.Id == 0)
            {
                var diary = _mapper.Map<Diary>(command);

                await _unitOfWork.Repository<Diary>().AddAsync(diary);
                await _unitOfWork.Commit(cancellationToken);
                return await Result<int>.SuccessAsync(diary.Id, _localizer["Diary Saved"]);
            }
            else
            {
                var diary = await _unitOfWork.Repository<Diary>().GetByIdAsync(command.Id);
                if (diary != null)
                {
                    diary.StartDateTime = command.StartDateTime;
                    diary.EndDateTime = command.EndDateTime;

                    diary.BookingId = command.BookingId == 0 ? diary.BookingId : command.BookingId;
                    diary.VehicleId = command.VehicleId == 0 ? diary.VehicleId : command.VehicleId;
                    diary.DiaryTypeId = command.DiaryTypeId == 0 ? diary.DiaryTypeId : command.DiaryTypeId;
                    await _unitOfWork.Repository<Diary>().UpdateAsync(diary);
                    await _unitOfWork.Commit(cancellationToken);
                    return await Result<int>.SuccessAsync(diary.Id, _localizer["Diary Updated"]);
                }
                else
                {
                    return await Result<int>.FailAsync(_localizer["Diary Not Found!"]);
                }
            }
        }
    }
}