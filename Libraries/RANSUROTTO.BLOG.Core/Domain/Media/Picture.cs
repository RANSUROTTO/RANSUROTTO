using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Media
{
    /// <summary>
    /// 图片文件
    /// </summary>
    public class Picture : BaseEntity
    {

        /// <summary>
        /// 获取或设置图片文件的MimeType类型
        /// </summary>
        public string MimeType { get; set; }

        /// <summary>
        /// 获取或设置图片SEO文件名
        /// </summary>
        public string SeoFilename { get; set; }

        /// <summary>
        /// 获取或设置图片的Alt属性
        /// </summary>
        public string AltAttribute { get; set; }

        /// <summary>
        /// 获取或设置图片的Title属性
        /// </summary>
        public string TitleAttribute { get; set; }

    }
}
