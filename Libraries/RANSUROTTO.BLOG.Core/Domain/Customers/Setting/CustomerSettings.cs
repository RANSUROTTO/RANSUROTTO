using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;

namespace RANSUROTTO.BLOG.Core.Domain.Customers.Setting
{
    public class CustomerSettings : ISettings
    {

        /// <summary>
        /// 获取或设置当前使用的登录类型
        /// </summary>
        public AuthenticationType CurrentAuthenticationType { get; set; }


    }

}
