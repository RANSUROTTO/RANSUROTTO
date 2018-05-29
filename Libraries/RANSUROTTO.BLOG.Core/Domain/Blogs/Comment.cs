using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Core.Domain.Blogs
{
    /// <summary>
    /// ��������
    /// </summary>
    public class Comment : BaseEntity
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
        public int? CustomerId { get; set; }

        /// <summary>
        /// ��ȡ����������ID
        /// </summary>
        public int BlogPostId { get; set; }

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
