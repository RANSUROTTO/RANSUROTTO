using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Core.Context
{
    public interface IWorkContext
    {

        /// <summary>
        /// 获取或设置工作区语言
        /// </summary>
        Language Language { get; set; }

    }

}
