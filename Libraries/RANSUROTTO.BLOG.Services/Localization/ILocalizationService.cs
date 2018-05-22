using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Services.Localization
{

    /// <summary>
    /// 区域化管理接口
    /// </summary>
    public interface ILocalizationService
    {

        /// <summary>
        /// 删除区域语言字符串资源
        /// </summary>
        /// <param name="localeStringResource">区域语言字符串资源</param>
        void DeleteLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// 获取区域设置区域语言字符串资源
        /// </summary>
        /// <param name="localeStringResourceId">区域语言字符串资源标识符</param>
        /// <returns>区域语言字符串资源</returns>
        LocaleStringResource GetLocaleStringResourceById(int localeStringResourceId);

        /// <summary>
        /// 通过资源名称与当前工作语言获取区域语言字符串资源
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <returns>区域语言字符串资源</returns>
        LocaleStringResource GetLocaleStringResourceByName(string resourceName);

        /// <summary>
        /// 获取区域语言字符串资源
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <param name="languageId">语言标识符</param>
        /// <param name="logIfNotFound">如果找不到区域语言字符串资源,指示是否要记录错误</param>
        /// <returns>区域语言字符串资源</returns>
        LocaleStringResource GetLocaleStringResourceByName(string resourceName, int languageId,
            bool logIfNotFound = true);

        /// <summary>
        /// 通过语言标识符获取所有区域语言字符串资源
        /// </summary>
        /// <param name="languageId">语言标识符</param>
        /// <returns>区域语言字符串资源列表</returns>
        IList<LocaleStringResource> GetAllResources(int languageId);

        /// <summary>
        /// 插入区域语言字符串资源
        /// </summary>
        /// <param name="localeStringResource">区域语言字符串资源</param>
        void InsertLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// 更新区域语言字符串资源
        /// </summary>
        /// <param name="localeStringResource">区域语言字符串资源</param>
        void UpdateLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// 通过语言标识符获取所有区域语言字符串资源
        /// </summary>
        /// <param name="languageId">语言标识符</param>
        /// <returns>语言与区域语言字符串资源字典</returns>
        Dictionary<string, KeyValuePair<int, string>> GetAllResourceValues(int languageId);

        /// <summary>
        /// 通过资源键值获取区域语言字符串资源值
        /// </summary>
        /// <param name="resourceKey">资源键值</param>
        /// <returns>区域语言字符串资源值</returns>
        string GetResource(string resourceKey);

        /// <summary>
        /// 通过资源键值获取区域语言字符串资源值
        /// </summary>
        /// <param name="resourceKey">资源键值</param>
        /// <param name="languageId">语言标识符</param>
        /// <param name="logIfNotFound">如果找不到区域语言字符串资源值,指示是否要记录错误</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="returnEmptyIfNotFound">指示未找到资源时是否返回空字符串,并将默认值设置为空字符串</param>
        /// <returns>区域语言字符串资源值</returns>
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
