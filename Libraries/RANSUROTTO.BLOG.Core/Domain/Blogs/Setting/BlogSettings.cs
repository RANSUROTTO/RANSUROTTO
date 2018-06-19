using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Blogs.Setting
{

    /// <summary>
    /// ��������
    /// </summary>
    public class BlogSettings : ISettings
    {

        /// <summary>
        /// ��ȡ�������Ƿ������ο�(�����û�)��������
        /// </summary>
        public bool AllowGuestsToCreateComments { get; set; }

        /// <summary>
        /// ��ȡ�����ò��Ŀ�������ǩ����
        /// </summary>  
        public int MaxNumberOfTags { get; set; }

        /// <summary>
        /// ��ȡ�����ò��������Ƿ���Ҫ��������
        /// </summary>
        public bool BlogCommentsMustBeApproved { get; set; }

        /// <summary>
        /// ��ȡ�����ø��ı��༭���Ƿ���Javascript֧��
        /// </summary>
        public bool RichEditorAllowJavaScript { get; set; }

        /// <summary>
        /// ��ȡ�����ø��ı��༭���ĸ�������
        /// </summary>
        public string RichEditorAdditionalSettings { get; set; }

        public bool RelativeDateTimeFormattingEnabled { get; set; }

        public bool AllowCustomersToEditComments { get; set; }

        public bool AllowCustomersToDeleteComments { get; set; }

        public bool NotifyAboutNewNewsComments { get; set; }

    }

}