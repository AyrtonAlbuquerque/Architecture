using Architecture.Api.Abstractions;
using Architecture.Api.Abstractions.Services;
using Architecture.Api.Attributes;
using Architecture.Api.Common;

namespace Architecture.Api.Infrastructure.Jobs
{
    [JobSchedule("0 * * * *")]
    public class UserJob(Settings settings, ILogService logService) : IJob
    {
        public async Task RunAsync()
        {
        }
    }
}