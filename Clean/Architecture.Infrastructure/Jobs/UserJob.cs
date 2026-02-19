using Architecture.Application.Abstractions;
using Architecture.Application.Abstractions.Services;
using Architecture.Application.Attributes;
using Architecture.Application.Common;

namespace Architecture.Infrastructure.Jobs
{
    [JobSchedule("0 * * * *")]
    public class UserJob(Settings settings, ILogService logService) : IJob
    {
        public async Task RunAsync()
        {
        }
    }
}