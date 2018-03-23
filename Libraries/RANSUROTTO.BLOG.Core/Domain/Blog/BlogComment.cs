using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Blog
{

    /// <summary>
    /// 博客评论
    /// </summary>
    public class BlogComment : BaseEntity
    {

        /// <summary>
        /// 获取或设置评论内容
        /// </summary>
        public string CommentText { get; set; }

        /// <summary>
        /// 获取或设置是否已经过审批
        /// </summary>
        public bool IsApproved { get; set; }

        /// <summary>
        /// 获取或设置用户ID
        /// </summary>
        public long? CustomerId { get; set; }

        /// <summary>
        /// 获取或设置文章ID
        /// </summary>
        public long BlogPostId { get; set; }

        #region Navigation Properties

        /// <summary>
        /// 获取或设置用户
        /// </summary>
        public virtual Customers.Customer Customer { get; set; }

        /// <summary>
        /// 获取或设置博文
        /// </summary>
        public virtual BlogPost BlogPost { get; set; }

        #endregion

    }

}
