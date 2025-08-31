using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Templify.Persistence.Settings
{
    public class EmailSettings
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Name { get; set; }
        public bool CheckCertificateRevocation { get; set; }
        public required string Host { get; set; }
        public int Port { get; set; }

        public EmailSettings()
        {
        }
        public EmailSettings(IConfiguration configuration)
        {
            var settings = configuration.GetSection("EmailServices");
            Email = settings["Email"] ?? throw new Exception($"Email configuration is missing in appsettings.json");
            Password = settings["Password"] ?? throw new Exception($"Email configuration is missing in appsettings.json");
            CheckCertificateRevocation = bool.Parse(settings["CheckCertificateRevocation"] ?? string.Empty);
            Host = settings["Host"] ?? throw new Exception($"Email configuration is missing in appsettings.json");
            Port = Convert.ToInt32(settings["Port"]);
        }
    }
}
