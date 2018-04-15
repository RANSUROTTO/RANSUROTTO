using RANSUROTTO.BLOG.Web.Models.Common;

namespace RANSUROTTO.BLOG.Web.Factories
{
    public interface ICommonModelFactory
    {

        /// <summary>
        /// 准备图标模型
        /// </summary>
        /// <returns>图标模型</returns>
        FaviconModel PrepareFaviconModel();

        /// <summary>
        /// 准备头部管理员链接模型
        /// </summary>
        /// <returns>头部管理员链接模型</returns>
        AdminHeaderLinksModel PrepareAdminHeaderLinksModel();

    }
}
