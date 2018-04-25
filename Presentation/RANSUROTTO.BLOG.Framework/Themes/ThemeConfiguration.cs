using System.Xml;

namespace RANSUROTTO.BLOG.Framework.Themes
{
    /// <summary>
    /// 主题配置
    /// </summary>
    public class ThemeConfiguration
    {
        public XmlNode ConfigurationNode { get; protected set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; protected set; }

        /// <summary>
        /// 预览图Url
        /// </summary>
        public string PreviewImageUrl { get; protected set; }

        /// <summary>
        /// 预览文本
        /// </summary>
        public string PreviewText { get; protected set; }

        /// <summary>
        /// 是否支持从右到左显示
        /// </summary>
        public bool SupportRtl { get; protected set; }

        /// <summary>
        /// 主题名称
        /// </summary>
        public string ThemeName { get; protected set; }

        /// <summary>
        /// 主题标题
        /// </summary>
        public string ThemeTitle { get; protected set; }

        public ThemeConfiguration(string themeName, string path, XmlDocument doc)
        {
            ThemeName = themeName;
            Path = path;
            var node = doc.SelectSingleNode("Theme");
            if (node != null)
            {
                ConfigurationNode = node;
                if (node.Attributes != null)
                {
                    var attribute = node.Attributes["title"];
                    ThemeTitle = attribute == null ? string.Empty : attribute.Value;
                    attribute = node.Attributes["supportRTL"];
                    SupportRtl = attribute != null && bool.Parse(attribute.Value);
                    attribute = node.Attributes["previewImageUrl"];
                    PreviewImageUrl = attribute == null ? string.Empty : attribute.Value;
                    attribute = node.Attributes["previewText"];
                    PreviewText = attribute == null ? string.Empty : attribute.Value;
                }
            }
        }

    }
}
