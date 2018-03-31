using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Core.Context
{

    /// <summary>
    /// 工作区上下文接口
    /// </summary>
    public interface IWorkContext
    {

        /// <summary>
        /// 获取或设置当前工作区语言
        /// </summary>
        Language WorkingLanguage { get; set; }

        /// <summary>
        /// 获取当前工作区用户
        /// </summary>
        Customer CurrentCustomer { get; set; }

        /// <summary>
        /// 获取或设置当前身份是否为管理员
        /// </summary>
        bool IsAdmin { get; set; }

    }

}
