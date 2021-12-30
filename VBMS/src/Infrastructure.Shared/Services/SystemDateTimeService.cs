using VBMS.Application.Interfaces.Services;
using System;

namespace VBMS.Infrastructure.Shared.Services
{
    public class SystemDateTimeService : IDateTimeService
    {
        public DateTime NowUtc => DateTime.UtcNow;
    }
}