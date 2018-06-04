using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Settings
{
    public class MediaSettingsModel : BaseModel
    {

        [ResourceDisplayName("Admin.Configuration.Settings.Media.MaximumImageSize")]
        public int MaximumImageSize { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Media.MultipleThumbDirectories")]
        public bool MultipleThumbDirectories { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Media.DefaultImageQuality")]
        public int DefaultImageQuality { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Media.DefaultPictureZoomEnabled")]
        public bool DefaultPictureZoomEnabled { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Media.AvatarPictureSize")]
        public int AvatarPictureSize { get; set; }

    }
}