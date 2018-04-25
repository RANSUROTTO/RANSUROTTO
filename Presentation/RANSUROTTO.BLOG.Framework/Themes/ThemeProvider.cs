using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using RANSUROTTO.BLOG.Core.Helper;

namespace RANSUROTTO.BLOG.Framework.Themes
{
    /// <summary>
    /// 主题提供商
    /// </summary>
    public class ThemeProvider : IThemeProvider
    {

        #region Fields

        private readonly IList<ThemeConfiguration> _themeConfigurations = new List<ThemeConfiguration>();
        private readonly string _basePath = string.Empty;

        #endregion

        #region Constructors

        public ThemeProvider()
        {
            _basePath = CommonHelper.MapPath("~/Themes/");
            LoadConfigurations();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 通过主题名称获取指定主题配置
        /// </summary>
        /// <param name="themeName">主题名称</param>
        /// <returns>主题配置</returns>
        public ThemeConfiguration GetThemeConfiguration(string themeName)
        {
            return _themeConfigurations
                .SingleOrDefault(x => x.ThemeName.Equals(themeName, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 获取所有主题配置
        /// </summary>
        /// <returns>主题配置列表</returns>
        public IList<ThemeConfiguration> GetThemeConfigurations()
        {
            return _themeConfigurations;
        }

        /// <summary>
        /// 获取一个值,标识主题配置列表中是否存在对应主题名称的主题配置
        /// </summary>
        /// <param name="themeName">主题名称</param>
        /// <returns>结果</returns>
        public bool ThemeConfigurationExists(string themeName)
        {
            return GetThemeConfigurations().Any(configuration => configuration.ThemeName.Equals(themeName, StringComparison.OrdinalIgnoreCase));
        }

        #endregion

        #region Utility

        /// <summary>
        /// 读取所有主题配置
        /// </summary>
        private void LoadConfigurations()
        {
            //TODO:Use IFileStorage?
            foreach (string themeName in Directory.GetDirectories(_basePath))
            {
                var configuration = CreateThemeConfiguration(themeName);
                if (configuration != null)
                {
                    _themeConfigurations.Add(configuration);
                }
            }
        }

        /// <summary>
        /// 通过主题文件夹路径获取对应主题下的主题配置
        /// </summary>
        /// <param name="themePath">主题文件夹路径</param>
        /// <returns>主题配置</returns>
        private ThemeConfiguration CreateThemeConfiguration(string themePath)
        {
            var themeDirectory = new DirectoryInfo(themePath);
            var themeConfigFile = new FileInfo(Path.Combine(themeDirectory.FullName, "theme.config"));

            if (themeConfigFile.Exists)
            {
                var doc = new XmlDocument();
                doc.Load(themeConfigFile.FullName);
                return new ThemeConfiguration(themeDirectory.Name, themeDirectory.FullName, doc);
            }

            return null;
        }

        #endregion

    }
}
