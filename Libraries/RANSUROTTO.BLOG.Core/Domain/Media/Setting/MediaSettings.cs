using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Media.Setting
{
    public class MediaSettings : ISettings
    {

        /// <summary>
        /// 获取或设置最大允许图片大小;如果上传的图片更大,则会调整大小。
        /// </summary>
        public int MaximumImageSize { get; set; }

        /// <summary>
        /// 获取或设置用于图像生成的质量
        /// </summary>
        public int DefaultImageQuality { get; set; }

        /// <summary>
        /// 获取或设置是否使用多个文件夹存储图片
        /// </summary>
        public bool MultipleThumbDirectories { get; set; }

        public bool DefaultPictureZoomEnabled { get; set; }

        public int AvatarPictureSize { get; set; }

    }
}
