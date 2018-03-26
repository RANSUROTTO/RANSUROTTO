namespace RANSUROTTO.BLOG.Core.Plugins
{
    public class BasePlugin : IPlugin
    {

        public virtual string GetConfigurationPageUrl()
        {
            return null;
        }
        public virtual PluginDescriptor PluginDescriptor { get; set; }

        public virtual void Install()
        {
            PluginManager.MarkPluginAsInstalled(this.PluginDescriptor.SystemName);
        }

        public virtual void Uninstall()
        {
            PluginManager.MarkPluginAsUninstalled(this.PluginDescriptor.SystemName);
        }

    }
}
