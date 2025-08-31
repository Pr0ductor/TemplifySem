using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Templify.Application.Interfaces.Services
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string message);
        public Task SendMessage(MimeMessage message);
    }

}
