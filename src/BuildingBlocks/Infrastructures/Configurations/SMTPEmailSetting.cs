using Contract.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Configurations
{
    public class SMTPEmailSetting : IEmailSMTPSetting
    {
        public string DisplayName { get; set; }
        public bool EnableVerification { get; set; }
        public string From { get; set; }
        public string SMTPServer { get; set; }
        public bool UseSsl { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
