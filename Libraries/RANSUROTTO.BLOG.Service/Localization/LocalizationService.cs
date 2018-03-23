using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using RANSUROTTO.BLOG.Core;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Domain.Localization.Setting;
using RANSUROTTO.BLOG.Data.Context;
using RANSUROTTO.BLOG.Service.Events;
using RANSUROTTO.BLOG.Service.Logging;

namespace RANSUROTTO.BLOG.Service.Localization
{
    public class LocalizationService : ILocalizationService
    {

        #region Constants

        /// <summary>
        /// 语言键缓存
        /// </summary>
        /// <remarks>
        /// {0} : 语言ID
        /// </remarks>
        private const string LOCALSTRINGRESOURCES_ALL_KEY = "Ransurotto.lsr.all-{0}";

        /// <summary>
        /// 语言键-资源键-缓存
        /// </summary>
        /// <remarks>
        /// {0} : 语言ID
        /// {1} : 资源键ID
        /// </remarks>
        private const string LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY = "Ransurotto.lsr.{0}-{1}";

        /// <summary>
        /// 语言资源缓存键清空匹配模式
        /// </summary>
        private const string LOCALSTRINGRESOURCES_PATTERN_KEY = "Ransurotto.lsr.";

        #endregion

        #region Fields

        private readonly LocalizationSettings _localizationSettings;
        private readonly IRepository<LocaleStringResource> _lsrRepository;
        private readonly IWorkContext _workContext;
        private readonly ILanguageService _languageService;
        private readonly ILogger _logger;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public LocalizationService(LocalizationSettings localizationSettings, IRepository<LocaleStringResource> lsrRepository, IWorkContext workContext, ILanguageService languageService, ILogger logger, ICacheManager cacheManager, IDataProvider dataProvider, IDbContext dbContext, IEventPublisher eventPublisher)
        {
            _localizationSettings = localizationSettings;
            _lsrRepository = lsrRepository;
            _workContext = workContext;
            _languageService = languageService;
            _logger = logger;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 删除区域语言字符串资源
        /// </summary>
        /// <param name="localeStringResource">区域语言字符串资源</param>
        public virtual void DeleteLocaleStringResource(LocaleStringResource localeStringResource)
        {
            if (localeStringResource == null)
                throw new ArgumentNullException(nameof(localeStringResource));

            _lsrRepository.Delete(localeStringResource);

            //删除缓存
            _cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);

            //发布删除通知
            _eventPublisher.EntityDeleted(localeStringResource);
        }

        /// <summary>
        /// 通过标识符获取区域语言字符串资源
        /// </summary>
        /// <param name="localeStringResourceId">区域语言字符串资源标识符</param>
        /// <returns>区域语言字符串资源</returns>
        public virtual LocaleStringResource GetLocaleStringResourceById(long localeStringResourceId)
        {
            if (localeStringResourceId == 0)
                return null;

            return _lsrRepository.GetById(localeStringResourceId);
        }

        /// <summary>
        /// 通过资源名称与当前工作语言获取区域语言字符串资源
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <returns>区域语言字符串资源</returns>
        public virtual LocaleStringResource GetLocaleStringResourceByName(string resourceName)
        {
            if (_workContext.WorkingLanguage != null)
                return GetLocaleStringResourceByName(resourceName, _workContext.WorkingLanguage.Id);

            return null;
        }

        /// <summary>
        /// 通过资源名称和语言标识符获取区域语言字符串资源
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <param name="languageId">语言标识符</param>
        /// <param name="logIfNotFound">如果找不到区域语言字符串资源,指示是否要记录错误</param>
        /// <returns>区域语言字符串资源</returns>
        public virtual LocaleStringResource GetLocaleStringResourceByName(string resourceName, long languageId,
            bool logIfNotFound = true)
        {
            var query = from lsr in _lsrRepository.Table
                        orderby lsr.ResourceName
                        where lsr.LanguageId == languageId && lsr.ResourceName == resourceName
                        select lsr;
            var localeStringResource = query.FirstOrDefault();

            if (localeStringResource == null && logIfNotFound)
                _logger.Warning($"资源字符串 ({resourceName}) 没有找到,语言标识符 = {languageId}");

            return localeStringResource;
        }

        /// <summary>
        /// 通过语言标识符获取对应的区域语言字符串资源列表
        /// </summary>
        /// <param name="languageId">语言标识符</param>
        /// <returns>区域语言字符串资源列表</returns>
        public virtual IList<LocaleStringResource> GetAllResources(long languageId)
        {
            var query = from l in _lsrRepository.Table
                        orderby l.ResourceName
                        where l.LanguageId == languageId
                        select l;
            var locales = query.ToList();
            return locales;
        }

        /// <summary>
        /// 添加区域语言字符串资源
        /// </summary>
        /// <param name="localeStringResource">区域语言字符串资源</param>
        public virtual void InsertLocaleStringResource(LocaleStringResource localeStringResource)
        {
            if (localeStringResource == null)
                throw new ArgumentNullException(nameof(localeStringResource));

            _lsrRepository.Insert(localeStringResource);

            //删除缓存
            _cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);

            //发布添加通知
            _eventPublisher.EntityInserted(localeStringResource);
        }

        /// <summary>
        /// 更新区域语言字符串资源
        /// </summary>
        /// <param name="localeStringResource">区域语言字符串资源</param>
        public virtual void UpdateLocaleStringResource(LocaleStringResource localeStringResource)
        {
            if (localeStringResource == null)
                throw new ArgumentNullException(nameof(localeStringResource));

            _lsrRepository.Update(localeStringResource);

            //删除缓存
            _cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);

            //发布更新通知
            _eventPublisher.EntityUpdated(localeStringResource);
        }

        /// <summary>
        /// 通过语言标识符获取所有区域语言字符串资源
        /// </summary>
        /// <param name="languageId">语言标识符</param>
        /// <returns>语言与区域语言字符串资源字典</returns>
        public virtual Dictionary<string, KeyValuePair<long, string>> GetAllResourceValues(long languageId)
        {
            string key = string.Format(LOCALSTRINGRESOURCES_ALL_KEY, languageId);
            return _cacheManager.Get(key, () =>
            {
                //当没有从缓存检索到该语言的区域语言字符串资源
                //则去数据库检索资源并且加入至缓存中
                var query = from l in _lsrRepository.TableNoTracking
                            orderby l.ResourceName
                            where l.LanguageId == languageId
                            select l;
                var locales = query.ToList();

                //格式: <name, <id, value>>
                //<资源名称,<资源id,资源值>>
                var dictionary = new Dictionary<string, KeyValuePair<long, string>>();
                foreach (var locale in locales)
                {
                    var resourceName = locale.ResourceName.ToLowerInvariant();
                    if (!dictionary.ContainsKey(resourceName))
                        dictionary.Add(resourceName, new KeyValuePair<long, string>(locale.Id, locale.ResourceValue));
                }
                return dictionary;
            });
        }

        /// <summary>
        /// 通过资源键值与当前工作语言获取区域语言字符串资源值
        /// </summary>
        /// <param name="resourceKey">资源键值</param>
        /// <returns>区域语言字符串资源值</returns>
        public virtual string GetResource(string resourceKey)
        {
            if (_workContext.WorkingLanguage != null)
                return GetResource(resourceKey, _workContext.WorkingLanguage.Id);

            return "";
        }

        /// <summary>
        /// 通过资源键值与语言标识符获取区域语言字符串资源值
        /// </summary>
        /// <param name="resourceKey">资源键值</param>
        /// <param name="languageId">语言标识符</param>
        /// <param name="logIfNotFound">如果找不到区域语言字符串资源值,指示是否要记录错误</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="returnEmptyIfNotFound">指示未找到资源时是否返回空字符串,并将默认值设置为空字符串</param>
        /// <returns>区域语言字符串资源值</returns>
        public virtual string GetResource(string resourceKey, long languageId,
            bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false)
        {
            string result = string.Empty;
            if (resourceKey == null)
                resourceKey = string.Empty;

            resourceKey = resourceKey.Trim().ToLowerInvariant();

            if (_localizationSettings.LoadAllLocaleRecordsOnStartup)
            {
                //加载所有区域语言字符串资源记录（我们知道它们是缓存的）
                var resources = GetAllResourceValues(languageId);
                if (resources.ContainsKey(resourceKey))
                {
                    result = resources[resourceKey].Value;
                }
            }
            else
            {
                //逐步加载
                string key = string.Format(LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY, languageId, resourceKey);
                string lsr = _cacheManager.Get(key, () =>
                {
                    var query = from l in _lsrRepository.Table
                                where l.ResourceName == resourceKey
                                && l.LanguageId == languageId
                                select l.ResourceValue;
                    return query.FirstOrDefault();
                });

                if (lsr != null)
                    result = lsr;
            }
            if (string.IsNullOrEmpty(result))
            {
                if (logIfNotFound)
                    _logger.Warning($"资源字符串 ({resourceKey}) 没有找到. 语言标识符 = {languageId}");

                if (!string.IsNullOrEmpty(defaultValue))
                {
                    result = defaultValue;
                }
                else
                {
                    if (!returnEmptyIfNotFound)
                        result = resourceKey;
                }
            }
            return result;
        }


        /// <summary>
        /// 将语言资源导出到xml
        /// </summary>
        /// <param name="language">语言</param>
        /// <returns>XML格式的结果</returns>
        public string ExportResourcesToXml(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            var sb = new StringBuilder();
            var stringWriter = new StringWriter(sb);
            var xmlWriter = new XmlTextWriter(stringWriter);
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Language");
            xmlWriter.WriteAttributeString("Name", language.Name);
            xmlWriter.WriteAttributeString("SupportedVersion", RansurottoBlogVersion.CurrentVersion);

            var resources = GetAllResources(language.Id);
            foreach (var resource in resources)
            {
                xmlWriter.WriteStartElement("LocaleResource");
                xmlWriter.WriteAttributeString("Name", resource.ResourceName);
                xmlWriter.WriteElementString("Value", null, resource.ResourceValue);
                xmlWriter.WriteEndElement();
            }

            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();

            return stringWriter.ToString();
        }

        /// <summary>
        /// 从XML文件导入语言资源
        /// </summary>
        /// <param name="language">语言</param>
        /// <param name="xml">XML格式的结果</param>
        /// <param name="updateExistingResources">指示是否更新现有资源的值</param>
        public void ImportResourcesFromXml(Language language, string xml, bool updateExistingResources = true)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            if (String.IsNullOrEmpty(xml))
                return;

            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var nodes = xmlDoc.SelectNodes(@"//Language/LocaleResource");
            foreach (XmlNode node in nodes)
            {
                string name = node.Attributes["Name"].InnerText.Trim();
                string value = "";
                var valueNode = node.SelectSingleNode("Value");

                if (valueNode != null)
                    value = valueNode.InnerText;

                if (string.IsNullOrEmpty(value))
                    continue;

                var resource = language.LocaleStringResources.FirstOrDefault(p => p.ResourceName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
                if (resource != null)
                {
                    if (updateExistingResources)
                        resource.ResourceValue = value;
                }
                else
                {
                    language.LocaleStringResources.Add(new LocaleStringResource
                    {
                        LanguageId = language.Id,
                        ResourceName = name,
                        ResourceValue = value
                    });
                }
            }
            _languageService.UpdateLanguage(language);

            //删除缓存
            _cacheManager.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
        }

        #endregion

    }
}
