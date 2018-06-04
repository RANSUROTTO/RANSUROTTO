using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Security.Setting
{
    public class CaptchaSettings : ISettings
    {

        public bool Enabled { get; set; }

        public bool ShowOnLoginPage { get; set; }

        public bool ShowOnRegistrationPage { get; set; }

        public bool ShowOnPublishBlogCommentPage { get; set; }

        public bool ShowOnPublishBlogPostPage { get; set; }

    }
}
