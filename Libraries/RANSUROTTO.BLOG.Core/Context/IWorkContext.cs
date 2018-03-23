using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Domain.Members;

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

    }

}
