using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;

namespace RANSUROTTO.BLOG.Web.Infrastructure.Installation
{
    public class InstallationLocalizationService : IInstallationLocalizationService
    {

        /// <summary>
        /// 安装页面使用语言的Cookie标记
        /// </summary>
        private const string LanguageCookieName = "ransurotto.installation.lang";

        /// <summary>
        /// 可用语言
        /// </summary>
        private IList<InstallationLanguage> _availableLanguages;

        /// <summary>
        /// 通过资源名称从当前运行时语言获取字符串资源值
        /// </summary>
        /// <param name="resourceName">资源名称</param>
        /// <returns>资源值</returns>
        public string GetResource(string resourceName)
        {
            var language = GetCurrentLanguage();
            if (language == null)
                return resourceName;

            var resourceValue = language.Resources
                .Where(r => r.Name.Equals(resourceName, StringComparison.OrdinalIgnoreCase))
                .Select(r => r.Value)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(resourceValue))
                return resourceName;

            return resourceValue;
        }

        /// <summary>
        /// 从当前运行时中获取指定语言
        /// </summary>
        /// <returns>语言</returns>
        public virtual InstallationLanguage GetCurrentLanguage()
        {
            var httpContext = EngineContext.Current.Resolve<HttpContextBase>();

            var cookieLanguageCode = "";
            var cookie = httpContext.Request.Cookies[LanguageCookieName];
            if (!string.IsNullOrEmpty(cookie?.Value))
                cookieLanguageCode = cookie.Value;

            //获取当前可用语言列表
            var availableLanguages = GetAvailableLanguages();

            //1.读取Cookie获得当前用户手动设置的语言
            var language = availableLanguages
                .FirstOrDefault(l => l.Code.Equals(cookieLanguageCode, StringComparison.OrdinalIgnoreCase));
            if (language != null)
                return language;

            //2.从浏览器文化设置中获得语言
            if (httpContext.Request.UserLanguages != null)
            {
                var userLanguage = httpContext.Request.UserLanguages.FirstOrDefault();
                if (!string.IsNullOrEmpty(userLanguage))
                {
                    //使用StartsWith来匹配语言代码
                    //假如浏览器首选语言为 zh-CN、则能查找到系统语言代码为 zh 的语言
                    language = availableLanguages
                        .FirstOrDefault(l => userLanguage.StartsWith(l.Code, StringComparison.InvariantCultureIgnoreCase));
                }
            }
            if (language != null)
                return language;

            //3.使用系统设置的默认语言
            language = availableLanguages.FirstOrDefault(l => l.IsDefault);
            if (language != null)
                return language;

            //4.返回语言列表的第一个语言
            language = availableLanguages.FirstOrDefault();
            return language;
        }

        /// <summary>
        /// 通过语言代码获取指定语言保存到当前环境
        /// Cookie
        /// </summary>
        /// <param name="languageCode">语言</param>
        public virtual void SaveCurrentLanguage(string languageCode)
        {
            var httpContext = EngineContext.Current.Resolve<HttpContextBase>();

            var cookie = new HttpCookie(LanguageCookieName);
            cookie.HttpOnly = true;
            cookie.Value = languageCode;
            cookie.Expires = DateTime.Now.AddHours(24);

            httpContext.Response.Cookies.Remove(LanguageCookieName);
            httpContext.Response.Cookies.Add(cookie);
        }

        /// <summary>
        /// 获取可用的语言列表
        /// </summary>
        /// <returns>可用的安装时语言列表</returns>
        public virtual IList<InstallationLanguage> GetAvailableLanguages()
        {
            if (_availableLanguages == null)
            {
                _availableLanguages = new List<InstallationLanguage>();
                foreach (var filePath in Directory.EnumerateFiles(CommonHelper.MapPath("~/App_Data/Localization/Installation/"), "*.xml"))
                {
                    var xmlDocument = new XmlDocument();
                    xmlDocument.Load(filePath);

                    //语言代码
                    var languageCode = "";
                    //文件名称格式: installation.{languagecode}.xml
                    var r = new Regex(Regex.Escape("installation.") + "(.*?)" + Regex.Escape(".xml"));
                    var matches = r.Matches(Path.GetFileName(filePath));
                    foreach (Match match in matches)
                        languageCode = match.Groups[1].Value;

                    //获取语言的友好名称
                    var languageName = xmlDocument.SelectSingleNode(@"//Language").Attributes["Name"].InnerText.Trim();

                    //默认选项
                    var isDefaultAttribute = xmlDocument.SelectSingleNode(@"//Language").Attributes["IsDefault"];
                    var isDefault = isDefaultAttribute != null && Convert.ToBoolean(isDefaultAttribute.InnerText.Trim());

                    //从右到左显示选项
                    var isRightToLeftAttribute = xmlDocument.SelectSingleNode(@"//Language").Attributes["IsRightToLeft"];
                    var isRightToLeft = isRightToLeftAttribute != null && Convert.ToBoolean(isRightToLeftAttribute.InnerText.Trim());

                    var language = new InstallationLanguage
                    {
                        Code = languageCode,
                        Name = languageName,
                        IsDefault = isDefault,
                        IsRightToLeft = isRightToLeft,
                    };
                    //加载该语言资源
                    foreach (XmlNode resNode in xmlDocument.SelectNodes(@"//Language/LocaleResource"))
                    {
                        var resNameAttribute = resNode.Attributes["Name"];
                        var resValueNode = resNode.SelectSingleNode("Value");

                        if (resNameAttribute == null)
                            throw new Exception("所有安装时可用语言资源都必须有属性:\"Name\".");
                        var resourceName = resNameAttribute.Value.Trim();

                        if (string.IsNullOrEmpty(resourceName))
                            throw new Exception("所有安装时可用语言资源属性 'Name' 必须要有值.");

                        if (resValueNode == null)
                            throw new Exception("所有安装时可用语言资源都必须有属性: \"Value\".");
                        var resourceValue = resValueNode.InnerText.Trim();

                        language.Resources.Add(new InstallationLocaleResource
                        {
                            Name = resourceName,
                            Value = resourceValue
                        });
                    }

                    _availableLanguages.Add(language);
                    _availableLanguages = _availableLanguages.OrderBy(l => l.Name).ToList();
                }
            }
            return _availableLanguages;
        }

    }
}