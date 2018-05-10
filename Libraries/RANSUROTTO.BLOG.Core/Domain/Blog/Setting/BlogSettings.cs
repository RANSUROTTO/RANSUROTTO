using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Blog.Setting
{

    /// <summary>
    /// ��������
    /// </summary>
    public class BlogSettings : ISettings
    {

        /// <summary>
        /// ��ȡ�������Ƿ�����δ�οͽ�������
        /// </summary>
        public bool AllowNotRegisteredUserToLeaveComments { get; set; }

        /// <summary>
        /// ��ȡ�����ò��Ŀ�������ǩ����
        /// </summary>
        public int MaxNumberOfTags { get; set; }

        /// <summary>
        /// ��ȡ�����ò��������Ƿ���Ҫ��������
        /// </summary>
        public bool BlogCommentsMustBeApproved { get; set; }

    }

}
