using System;
using System.Xml;

namespace RANSUROTTO.BLOG.Core.Configuration
{
    public class WebConfig : BaseConfig<WebConfig>
    {

        /// <summary>
        /// 创建一个配置处理程序
        /// </summary>
        /// <param name="section">配置文件WebCofig xml父节点</param>
        public override WebConfig Create(XmlNode section)
        {
            Config = new WebConfig();

            var startupNode = section.SelectSingleNode("Startup");
            Config.IgnoreStartupTasks = GetBool(startupNode, "IgnoreStartupTasks");

            var redisCachingNode = section.SelectSingleNode("RedisCaching");
            Config.RedisCachingEnable = GetBool(redisCachingNode, "Enable");
            Config.RedisCachingConfig = GetString(redisCachingNode, "ConfigString");

            var webFarmsNode = section.SelectSingleNode("WebFarms");
            Config.MultipleInstancesEnabled = GetBool(webFarmsNode, "MultipleInstancesEnabled");
            Config.RunOnAzureWebApps = GetBool(webFarmsNode, "RunOnAzureWebApps");

            var userAgentStringsNode = section.SelectSingleNode("UserAgentStrings");
            Config.UserAgentStringsPath = GetString(userAgentStringsNode, "databasePath");
            Config.CrawlerOnlyUserAgentStringsPath = GetString(userAgentStringsNode, "crawlersOnlyDatabasePath");

            var installationNode = section.SelectSingleNode("Installation");
            Config.DisableSampleDataDuringInstallation = GetBool(installationNode, "DisableSampleDataDuringInstallation");

            return Config;
        }

        #region Properties

        /// <summary>
        /// 是否忽略运行应用程序启动任务
        /// </summary>
        public bool IgnoreStartupTasks { get; private set; }

        /// <summary>
        /// 标识是否开启Redis缓存
        /// </summary>
        public bool RedisCachingEnable { get; private set; }

        /// <summary>
        /// Redis缓存配置字符串
        /// </summary>
        public string RedisCachingConfig { get; private set; }

        /// <summary>
        /// 标识该站点是否在多个实例上运行(例如Web站点、多实例WindowsAzure、集群概念)
        /// </summary>
        public bool MultipleInstancesEnabled { get; private set; }

        /// <summary>
        /// 标识是否在应用程序启动时清空插件的bin目录
        /// </summary>
        public bool ClearPluginShadowDirectoryOnStartup { get; private set; }

        /// <summary>
        /// 标识应用程序是否在Windows Azure Web Apps上运行
        /// </summary>
        public bool RunOnAzureWebApps { get; private set; }

        /// <summary>
        /// 包含用户代理字符串的数据库路径
        /// </summary>
        public string UserAgentStringsPath { get; private set; }

        /// <summary>
        /// 使用仅搜寻器用户代理字符串的数据库路径
        /// </summary>
        public string CrawlerOnlyUserAgentStringsPath { get; private set; }

        /// <summary>
        /// 标识是否禁用应用安装页面"导入测试数据"选项
        /// </summary>
        public bool DisableSampleDataDuringInstallation { get; private set; }

        #endregion

        #region Utilities

        /// <summary>
        /// 获取节点属性值 转字符串
        /// </summary>
        private string GetString(XmlNode node, string attrName)
        {
            return SetByXElement<string>(node, attrName, Convert.ToString);
        }

        /// <summary>
        /// 获取节点属性值 转布尔
        /// </summary>
        private bool GetBool(XmlNode node, string attrName)
        {
            return SetByXElement<bool>(node, attrName, Convert.ToBoolean);
        }

        /// <summary>
        /// 获取节点属性值 通过自定义转换函数转为指定类型
        /// </summary>
        private T SetByXElement<T>(XmlNode node, string attrName, Func<string, T> converter)
        {
            if (node?.Attributes == null) return default(T);
            var attr = node.Attributes[attrName];
            if (attr == null) return default(T);
            var attrVal = attr.Value;
            return converter(attrVal);
        }

        #endregion

    }
}
