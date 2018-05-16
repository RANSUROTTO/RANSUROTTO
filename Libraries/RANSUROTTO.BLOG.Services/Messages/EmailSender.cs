using System.Net;
using System.Linq;
using System.Net.Mail;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Messages;

namespace RANSUROTTO.BLOG.Services.Messages
{
    public class EmailSender : IEmailSender
    {

        public void SendEmail(EmailAccount emailAccount, string subject, string body, string fromAddress, string fromName,
            string toAddress, string toName, string replyToAddress = null, string replyToName = null, IEnumerable<string> bcc = null,
            IEnumerable<string> cc = null, string attachmentFilePath = null, string attachmentFileName = null,
            int attachedDownloadId = 0, IDictionary<string, string> headers = null)
        {
            var message = new MailMessage();

            //添加发件人、收件人、回复人、抄送人信息
            message.From = new MailAddress(fromAddress, fromName);
            message.To.Add(new MailAddress(toAddress, toName));
            if (!string.IsNullOrEmpty(replyToAddress))
                message.ReplyToList.Add(new MailAddress(replyToAddress, replyToName));
            if (bcc != null)
                foreach (var address in bcc.Where(bccValue => !string.IsNullOrWhiteSpace(bccValue)))
                    message.Bcc.Add(address.Trim());
            if (cc != null)
                foreach (var address in cc.Where(ccValue => !string.IsNullOrWhiteSpace(ccValue)))
                    message.CC.Add(address.Trim());

            message.Subject = subject;
            message.Body = body;
            //使用html格式内容邮件
            message.IsBodyHtml = true;

            if (headers != null)
                foreach (var header in headers)
                    message.Headers.Add(header.Key, header.Value);

            #region 附件处理

            //TODO 电子邮件发送增加下载附件处理

            #endregion

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.UseDefaultCredentials = emailAccount.UseDefaultCredentials;
                smtpClient.Host = emailAccount.Host;
                smtpClient.Port = emailAccount.Port;
                smtpClient.EnableSsl = emailAccount.EnableSsl;
                smtpClient.Credentials = emailAccount.UseDefaultCredentials ?
                    CredentialCache.DefaultNetworkCredentials :
                    new NetworkCredential(emailAccount.Username, emailAccount.Password);
                smtpClient.Send(message);
            }
        }

    }
}
