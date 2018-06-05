using RANSUROTTO.BLOG.Framework.Mvc;

namespace RANSUROTTO.BLOG.Admin.Models.Settings
{
    public class BlogSettingsModel : BaseModel
    {

        public bool AllowGuestsToCreateComments { get; set; }

        public int MaxNumberOfTags { get; set; }

        public bool RichEditorAllowJavaScript { get; set; }

        public string RichEditorAdditionalSettings { get; set; }

        public bool RelativeDateTimeFormattingEnabled { get; set; }

        public bool BlogCommentsMustBeApproved { get; set; }

        public bool AllowCustomersToEditComments { get; set; }

        public bool AllowCustomersToDeleteComments { get; set; }

        public bool NotifyAboutNewNewsComments { get; set; }

    }
}