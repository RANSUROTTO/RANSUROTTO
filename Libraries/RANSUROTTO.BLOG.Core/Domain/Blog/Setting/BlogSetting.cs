namespace RANSUROTTO.BLOG.Core.Domain.Blog.Setting
{

    public class BlogSetting
    {

        /// <summary>
        /// ��ȡ�������Ƿ�����δע���û�(�ο�)��������
        /// </summary>
        public bool AllowNotRegisteredUserToLeaveComments { get; set; }

        /// <summary>
        /// ��ȡ�����ò��Ŀ�������ǩ����
        /// </summary>
        public int MaxNumberOfTags { get; set; }

    }

}
