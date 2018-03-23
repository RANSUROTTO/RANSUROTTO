using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Core.Context
{
    public interface IWorkContext
    {

        /// <summary>
        /// 获取或设置当前工作语言
        /// </summary>
        Language WorkingLanguage { get; set; }

    }

}
