using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Configurations
{
    public  class MasstransitRabitMQSettings
    {
        public string Host { get; set; }
        public string VHost { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string ExchangeName { get; set; }
        public string ExchangeType { get; set; }
        public string RequestServiceQueue { get; set; }
        public string SagaQueue { get; set; }
    }
}
