using System.Collections.Generic;

namespace RANSUROTTO.BLOG.Framework.Themes
{
    /// <summary>
    /// 主题提供商接口
    /// </summary>
    public interface IThemeProvider
    {

        /// <summary>
        /// 通过主题名称获取指定主题配置
        /// </summary>
        /// <param name="themeName">主题名称</param>
        /// <returns>主题配置</returns>
        ThemeConfiguration GetThemeConfiguration(string themeName);

        /// <summary>
        /// 获取所有主题配置
        /// </summary>
        /// <returns>主题配置列表</returns>
        IList<ThemeConfiguration> GetThemeConfigurations();

        /// <summary>
        /// 获取一个值,标识主题配置列表中是否存在对应主题名称的主题配置
        /// </summary>
        /// <param name="themeName">主题名称</param>
        /// <returns>结果</returns>
        bool ThemeConfigurationExists(string themeName);

    }
}
