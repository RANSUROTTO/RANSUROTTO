namespace RANSUROTTO.BLOG.Core.Plugins
{
    public interface IPlugin
    {

        /// <summary>
        /// 获取配置页面路径
        /// </summary>
        string GetConfigurationPageUrl();

        /// <summary>
        /// 获取插件的信息
        /// </summary>
        PluginDescriptor PluginDescriptor { get; set; }

        /// <summary>
        /// 安装插件
        /// </summary>
        void Install();

        /// <summary>
        /// 卸载插件
        /// </summary>
        void Uninstall();

    }
}
