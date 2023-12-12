using Contract.Services;
using Shared.Services.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Contract.Configurations;
using MimeKit;
using MailKit.Net.Smtp;
using Infrastructures.Configurations;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Threading;

namespace Infrastructures.Services
{
    public class SmtpEmailService : ISmtpEmailService
    {
        private readonly ILogger _logger;
        private readonly SMTPEmailSetting _emailSetting;
        private readonly SmtpClient _smtpClient;

        public SmtpEmailService(ILogger logger, SMTPEmailSetting emailSetting)
        {
            _logger = logger;
            _emailSetting = emailSetting;
            _smtpClient = new SmtpClient();
        }

        public void SendEmail(MailRequest request)
        {
            var emailMessage = getMimeMessage(request);

            try
            {
                 _smtpClient.Connect(_emailSetting.SMTPServer, _emailSetting.Port,
                    _emailSetting.UseSsl);
                 _smtpClient.Authenticate(_emailSetting.UserName, _emailSetting.Password);
                 _smtpClient.Send(emailMessage);
                 _smtpClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            finally
            {
                 _smtpClient.Disconnect(true);
                _smtpClient.Dispose();
            }
        }

        public async Task SendEmailAsync(MailRequest request, CancellationToken cancellationToken = default)
        {
            var emailMessage = getMimeMessage(request);

            try
            {
                await _smtpClient.ConnectAsync(_emailSetting.SMTPServer, _emailSetting.Port,
                    _emailSetting.UseSsl, cancellationToken);
                await _smtpClient.AuthenticateAsync(_emailSetting.UserName, _emailSetting.Password, cancellationToken);
                await _smtpClient.SendAsync(emailMessage, cancellationToken);
                await _smtpClient.DisconnectAsync(true, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, ex.Message);
            }
            finally
            {
                await _smtpClient.DisconnectAsync(true, cancellationToken);
                _smtpClient.Dispose();
            }

        }

        private MimeMessage getMimeMessage(MailRequest request)
        {
            var emailMessage = new MimeMessage
            {
                Sender = new MailboxAddress(_emailSetting.DisplayName, request.From ?? _emailSetting.From),
                Subject = request.Subject,
                Body = new BodyBuilder
                {
                    HtmlBody = request.Body
                }.ToMessageBody()
            };

            if (request.ToAddresses.Any())
            {
                foreach (var toAddress in request.ToAddresses)
                {
                    emailMessage.To.Add(MailboxAddress.Parse(toAddress));
                }
            }
            else
            {
                var toAddress = request.ToAddress;
                emailMessage.To.Add(MailboxAddress.Parse(toAddress));
            }

            return emailMessage;
        }
    }
}
