using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Messages.Setting
{
    public class EmailAccountSettings : ISettings
    {

        /// <summary>
        /// 获取或设置应用程序默认使用的邮箱账户的标识符
        /// </summary>
        public long DefaultEmailAccountId { get; set; }

    }
}
