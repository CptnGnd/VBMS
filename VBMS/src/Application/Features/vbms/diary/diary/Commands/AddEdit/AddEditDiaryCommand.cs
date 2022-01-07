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

namespace VBMS.Application.Features.vbms.diary.diary.Commands.AddEdit
{
    public partial class AddEditDiaryCommand : IRequest<Result<int>>
    {
        public int Id { get; set; }
        [Required]
        public DateTime StartDateTime { get; set; }
        [Required]
        public DateTime EndDateTime { get; set; }
        public int BookingId { get; set; }
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
            if (await _unitOfWork.Repository<Diary>().Entities.Where(p => p.Id != command.Id)
                .AnyAsync(p => p.VehicleId == command.VehicleId && p.StartDateTime <= command.EndDateTime && p.EndDateTime >= command.StartDateTime, cancellationToken))
            {
                return await Result<int>.FailAsync(_localizer["Diary Redcords may not Overlap"]);
            }


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
                    diary.BookingId = command.VehicleId == 0 ? diary.DiaryTypeId : command.DiaryTypeId;
                    diary.BookingId = command.DiaryTypeId == 0 ? diary.DiaryTypeId : command.DiaryTypeId;
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