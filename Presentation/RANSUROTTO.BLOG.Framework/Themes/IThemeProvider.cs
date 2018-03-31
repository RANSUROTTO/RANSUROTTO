using System.Collections.Generic;

namespace RANSUROTTO.BLOG.Framework.Themes
{
    public interface IThemeProvider
    {

        ThemeConfiguration GetThemeConfiguration(string themeName);

        IList<ThemeConfiguration> GetThemeConfigurations();

        bool ThemeConfigurationExists(string themeName);

    }
}
