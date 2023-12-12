using Contract.ScheduledJobs;
using Contract.Services;
using Hangfire.Api.Services.Interfaces;
using Shared.Services.Email;
using ILogger = Serilog.ILogger;

namespace Hangfire.Api.Services
{
    public class BackGroundJobService : IBackGroundJobService
    {
        private readonly IScheduledJobService _jobservice;
        private readonly ISmtpEmailService _emailService;
        private readonly ILogger _logger;

        public BackGroundJobService(IScheduledJobService jobservice, ISmtpEmailService emailService, ILogger logger)
        {
            _jobservice = jobservice;
            _emailService = emailService;
            _logger = logger;
        }

        public IScheduledJobService ScheduledJobService => _jobservice;

        public string? SendEmailContent(string email, string subject, string emailContent, DateTimeOffset enqueueAt)
        {
            var emailRequets = new MailRequest
            {
                ToAddress = email,
                Body = emailContent,
                Subject = subject
            };

            try
            {
                var jobId = _jobservice.Schedule(() => _emailService.SendEmail(emailRequets), enqueueAt);
                _logger.Information($"Sent Email to {email} with subject {subject} - jobId {jobId}");
                return jobId;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed due to an error the email service {ex.Message}");
                return null;
            }
        }
    }
}
