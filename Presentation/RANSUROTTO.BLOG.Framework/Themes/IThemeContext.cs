namespace RANSUROTTO.BLOG.Framework.Themes
{
    /// <summary>
    /// 主题上下文
    /// </summary>
    public interface IThemeContext
    {

        /// <summary>
        /// 获取或设置当前主题系统名称
        /// </summary>
        string WorkingThemeName { get; set; }

    }
}
