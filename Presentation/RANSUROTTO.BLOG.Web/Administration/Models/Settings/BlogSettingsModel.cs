using RANSUROTTO.BLOG.Framework.Localization;
using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Settings
{
    public class BlogSettingsModel : BaseModel
    {

        [ResourceDisplayName("Admin.Configuration.Settings.Blog.AllowGuestsToCreateComments")]
        public bool AllowGuestsToCreateComments { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Blog.MaxNumberOfTags")]
        public int MaxNumberOfTags { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Blog.RichEditorAllowJavaScript")]
        public bool RichEditorAllowJavaScript { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Blog.RichEditorAdditionalSettings")]
        public string RichEditorAdditionalSettings { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Blog.RelativeDateTimeFormattingEnabled")]
        public bool RelativeDateTimeFormattingEnabled { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Blog.BlogCommentsMustBeApproved")]
        public bool BlogCommentsMustBeApproved { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Blog.AllowCustomersToEditComments")]
        public bool AllowCustomersToEditComments { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Blog.AllowCustomersToDeleteComments")]
        public bool AllowCustomersToDeleteComments { get; set; }

        [ResourceDisplayName("Admin.Configuration.Settings.Blog.NotifyAboutNewNewsComments")]
        public bool NotifyAboutNewNewsComments { get; set; }

    }
}