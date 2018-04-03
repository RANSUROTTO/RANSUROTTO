using System.Collections.Generic;

namespace RANSUROTTO.BLOG.Web.Infrastructure.Installation
{
    /// <summary>
    /// 安装服务区域化接口
    /// </summary>
    public interface IInstallationLocalizationService
    {

        /// <summary>
        /// 通过资源名称从当前运行时语言获取字符串资源值
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <returns>资源值</returns>
        string GetResource(string resourceName);

        /// <summary>
        /// 从当前运行时中获取指定语言
        /// </summary>
        /// <returns>语言</returns>
        InstallationLanguage GetCurrentLanguage();

        /// <summary>
        /// 通过语言代码获取指定语言保存到当前运行时
        /// </summary>
        /// <param name="languageCode">语言</param>
        void SaveCurrentLanguage(string languageCode);

        /// <summary>
        /// 获取可用的语言列表
        /// </summary>
        /// <returns>可用的安装时语言列表</returns>
        IList<InstallationLanguage> GetAvailableLanguages();

    }
}
