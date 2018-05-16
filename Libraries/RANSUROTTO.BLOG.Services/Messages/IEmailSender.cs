using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RANSUROTTO.BLOG.Core.Domain.Messages;

namespace RANSUROTTO.BLOG.Services.Messages
{
    /// <summary>
    /// 邮件发送接口
    /// </summary>
    public interface IEmailSender
    {

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="emailAccount">发送邮件账户</param>
        /// <param name="subject">主题</param>
        /// <param name="body">内容</param>
        /// <param name="fromAddress">发件人地址</param>
        /// <param name="fromName">发件人名称</param>
        /// <param name="toAddress">收件人地址</param>
        /// <param name="toName">收件人名称</param>
        /// <param name="replyToAddress">邮件回复地址</param>
        /// <param name="replyToName">邮件回复地址收件人名称</param>
        /// <param name="bcc">电子邮件的密件抄送 (BCC) 收件人的地址集合</param>
        /// <param name="cc">电子邮件的抄送 (CC) 收件人的地址集合</param>
        /// <param name="attachmentFilePath">附件文件路径</param>
        /// <param name="attachmentFileName">附件文件明;如果指定,则该文件将发送给收件人;否则,将使用'attachmentFilePath'原附件文件名</param>
        /// <param name="attachedDownloadId">附件下载ID（另附）</param>
        /// <param name="headers">电子邮件标头</param>
        void SendEmail(EmailAccount emailAccount, string subject, string body,
            string fromAddress, string fromName, string toAddress, string toName,
            string replyToAddress = null, string replyToName = null,
            IEnumerable<string> bcc = null, IEnumerable<string> cc = null,
            string attachmentFilePath = null, string attachmentFileName = null,
            int attachedDownloadId = 0, IDictionary<string, string> headers = null);

    }
}
