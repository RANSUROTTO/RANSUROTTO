using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Core.Domain.Blogs
{
    /// <summary>
    /// ��������
    /// </summary>
    public class BlogComment : BaseEntity
    {

        /// <summary>
        /// ��ȡ��������������
        /// </summary>
        public string CommentText { get; set; }

        /// <summary>
        /// ��ȡ�������Ƿ��Ѿ�������
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// ��ȡ�������û�ID
        /// </summary>
        public long? CustomerId { get; set; }

        /// <summary>
        /// ��ȡ����������ID
        /// </summary>
        public long BlogPostId { get; set; }

        #region Navigation Properties

        /// <summary>
        /// ��ȡ�������û�
        /// </summary>
        public virtual Customer Customer { get; set; }

        /// <summary>
        /// ��ȡ�����ò���
        /// </summary>
        public virtual BlogPost BlogPost { get; set; }

        #endregion

    }
}
