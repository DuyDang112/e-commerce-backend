using Contract.ScheduledJobs;

namespace Hangfire.Api.Services.Interfaces
{
    public interface IBackGroundJobService
    {
        IScheduledJobService ScheduledJobService { get; }

        string? SendEmailContent(string email, string subject, 
            string emailContent, DateTimeOffset enqueueAt);


    }
}
