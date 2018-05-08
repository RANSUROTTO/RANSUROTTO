using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Messages;

namespace RANSUROTTO.BLOG.Services.Messages
{
    public interface IEmailAccountService
    {

        /// <summary>
        /// 添加邮箱账户
        /// </summary>
        /// <param name="emailAccount">邮箱账户</param>
        void InsertEmailAccount(EmailAccount emailAccount);

        /// <summary>
        /// 更新邮箱账户
        /// </summary>
        /// <param name="emailAccount">邮箱账户</param>
        void UpdateEmailAccount(EmailAccount emailAccount);

        /// <summary>
        /// 删除邮箱账户
        /// </summary>
        /// <param name="emailAccount">邮箱账户</param>
        void DeleteEmailAccount(EmailAccount emailAccount);

        /// <summary>
        /// 通过标识符获取邮箱账户
        /// </summary>
        /// <param name="emailAccountId">邮箱账户标识符</param>
        /// <returns>邮箱账户</returns>
        EmailAccount GetEmailAccountById(long emailAccountId);

        /// <summary>
        /// 获取所有邮箱账户
        /// </summary>
        /// <returns>邮箱账户列表</returns>
        IList<EmailAccount> GetAllEmailAccounts();

    }
}
