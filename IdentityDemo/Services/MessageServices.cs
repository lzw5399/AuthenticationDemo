using IdentityDemo.Models;
using Microsoft.Extensions.Options;
using qcloudsms_csharp;
using qcloudsms_csharp.httpclient;
using qcloudsms_csharp.json;
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
        private readonly MailSenderOptions _mailSenderOptions;
        private readonly SmsSenderOptions _smsSenderOptions;

        public AuthMessageSender(IOptions<MailSenderOptions> mailOptions, IOptions<SmsSenderOptions> smsOptions)
        {
            _mailSenderOptions = mailOptions.Value;
            _smsSenderOptions = smsOptions.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            // 发送邮件可以直接用System.Net.Mail，但是很复杂,但是这里用这个
            // 也可以用第三方库，MailKit
            var client = new SmtpClient(_mailSenderOptions.Host);
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(_mailSenderOptions.UserName, _mailSenderOptions.Password);

            var mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(_mailSenderOptions.UserName);
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = message;

            return client.SendMailAsync(mailMessage);
        }

        public Task SendSmsAsync(string number, string message)
        {
            int appid = _smsSenderOptions.AppId;

            string appkey = _smsSenderOptions.AppKey;

            var phoneNumbers = new string[] { number };

            int templateId = _smsSenderOptions.TemplateId;

            string smsSign = _smsSenderOptions.SmsSign; // 签名的参数(codepie)

            try
            {
                // 签名参数未提供或者为空时，会使用默认签名发送短信
                var ssender = new SmsSingleSender(appid, appkey);
                SmsSingleSenderResult result = ssender.sendWithParam("86", phoneNumbers[0],
                    templateId, new[] { "5678", "tes-.-" }, smsSign, "", "");
            }
            catch (JSONException e)
            {
                Console.WriteLine(e);
            }
            catch (HTTPException e)
            {
                Console.WriteLine(e);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return Task.CompletedTask;
        }
    }
}
