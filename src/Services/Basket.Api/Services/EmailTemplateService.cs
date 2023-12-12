using Shared.Configurations;
using System.Text;

namespace Basket.Api.Services
{
    public class EmailTemplateService 
    {
        protected readonly BackgroundJobSettings _jobSettings;


        private static readonly string _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        private static readonly string _tmpFolder = Path.Combine(_baseDirectory, "EmailTemplates");

        public EmailTemplateService(BackgroundJobSettings jobSettings)
        {
            _jobSettings = jobSettings;
        }

        protected string ReadEmailTemplateContent(string templateEmailName, string format = "html")
        {
            var filePath = Path.Combine(_tmpFolder, templateEmailName + "." + format);
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var sr = new StreamReader(fs, Encoding.Default);
            var emailText = sr.ReadToEnd();
            sr.Close();

            return emailText;
        }
    }
}
