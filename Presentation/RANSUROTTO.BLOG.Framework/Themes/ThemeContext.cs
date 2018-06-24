using System;
using System.Linq;
using RANSUROTTO.BLOG.Core.Domain.Common.Setting;

namespace RANSUROTTO.BLOG.Framework.Themes
{
    public class ThemeContext : IThemeContext
    {

        private readonly IThemeProvider _themeProvider;
        private readonly CommonSettings _commonSettings;

        private bool _themeIsCached;
        private string _cachedThemeName;

        public ThemeContext(IThemeProvider themeProvider, CommonSettings commonSettings)
        {
            _themeProvider = themeProvider;
            _commonSettings = commonSettings;
        }

        public string WorkingThemeName
        {
            get
            {
                if (_themeIsCached)
                    return _cachedThemeName;

                var theme = "";

                //TODO 是否允许用户自己选择主题

                if (string.IsNullOrEmpty(theme))
                    theme = _commonSettings.DefaultTheme;

                if (!_themeProvider.ThemeConfigurationExists(theme))
                {
                    var themeInstance = _themeProvider.GetThemeConfigurations()
                        .FirstOrDefault();
                    if (themeInstance == null)
                        throw new Exception("没有主题可以加载");
                    theme = themeInstance.ThemeName;
                }
                this._cachedThemeName = theme;
                this._themeIsCached = true;

                return theme;
            }
            set
            {
                //TODO 未确认进行设置主题处理
                //TODO 是否允许用户自己选择主题
                this._themeIsCached = false;
            }
        }

    }
}
