using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Service.Localization
{

    /// <summary>
    /// 区域化管理接口
    /// </summary>
    public interface ILocalizationService
    {

        /// <summary>
        /// 删除区域设置语言字符串资源
        /// </summary>
        /// <param name="localeStringResource">区域资源字符串</param>
        void DeleteLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// 获取区域设置字符串资源
        /// </summary>
        /// <param name="localeStringResourceId">字符串资源标识符</param>
        /// <returns>区域资源字符串</returns>
        LocaleStringResource GetLocaleStringResourceById(int localeStringResourceId);

        /// <summary>
        /// 获取区域设置字符串资源
        /// </summary>
        /// <param name="resourceName">表示资源名的字符串</param>
        /// <returns>区域资源字符串</returns>
        LocaleStringResource GetLocaleStringResourceByName(string resourceName);

        /// <summary>
        /// 获取区域设置字符串资源
        /// </summary>
        /// <param name="resourceName">表示资源名的字符串。</param>
        /// <param name="languageId">语言标识符</param>
        /// <param name="logIfNotFound">如果找不到区域字符串资源,指示是否要记录错误</param>
        /// <returns>区域资源字符串</returns>
        LocaleStringResource GetLocaleStringResourceByName(string resourceName, int languageId,
            bool logIfNotFound = true);

        /// <summary>
        /// 通过语言标识符获取所有区域字符串资源
        /// </summary>
        /// <param name="languageId">语言标识符</param>
        /// <returns>区域资源字符串</returns>
        IList<LocaleStringResource> GetAllResources(int languageId);

        /// <summary>
        /// 插入区域设置字符串资源
        /// </summary>
        /// <param name="localeStringResource">区域资源字符串</param>
        void InsertLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// 更新区域设置字符串资源
        /// </summary>
        /// <param name="localeStringResource">区域资源字符串</param>
        void UpdateLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// 通过语言标识符获取所有区域字符串资源
        /// </summary>
        /// <param name="languageId">语言标识符</param>
        /// <returns>区域资源字符串</returns>
        Dictionary<string, KeyValuePair<int, string>> GetAllResourceValues(int languageId);

        /// <summary>
        /// 通过资源键值获取区域字符串
        /// </summary>
        /// <param name="resourceKey">资源键值</param>
        /// <returns>区域字符串</returns>
        string GetResource(string resourceKey);

        /// <summary>
        /// 通过资源键值获取区域字符串
        /// </summary>
        /// <param name="resourceKey">资源键值</param>
        /// <param name="languageId">语言标识符</param>
        /// <param name="logIfNotFound">如果找不到区域字符串资源,指示是否要记录错误</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="returnEmptyIfNotFound">指示未找到资源时是否返回空字符串,并将默认值设置为空字符串</param>
        /// <returns>区域字符串</returns>
        string GetResource(string resourceKey, int languageId,
            bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false);

        /// <summary>
        /// 将语言资源导出到xml
        /// </summary>
        /// <param name="language">语言</param>
        /// <returns>XML格式的结果</returns>
        string ExportResourcesToXml(Language language);

        /// <summary>
        /// 从XML文件导入语言资源
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="xml">XML格式的结果</param>
        /// <param name="updateExistingResources">指示是否更新现有资源的值</param>
        void ImportResourcesFromXml(Language language, string xml, bool updateExistingResources = true);

    }

}
