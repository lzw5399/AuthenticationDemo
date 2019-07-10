using IdentityDemo.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityDemo.Services
{
    // 这里实现我们自己的发送邮件和短信的功能，用自己提供商
    // 邮箱需要邮箱服务器和自己的邮箱地址
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly SenderOptions _senderOptions;

        public AuthMessageSender(IOptions<SenderOptions> options)
        {
            _senderOptions = options.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // 发送邮件可以直接用System.Net.Mail，但是很复杂,但是这里用这个
            // 也可以用第三方库，MailKit
            var client = new SmtpClient(_senderOptions.Host);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_senderOptions.UserName, _senderOptions.Password);

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_senderOptions.UserName);
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = message;

            return client.SendMailAsync(mailMessage);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // 短信同样需要提供商
            return Task.FromResult(0);
        }
    }
}
