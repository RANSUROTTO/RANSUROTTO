using System;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Blogs.Enum;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Core.Domain.Blogs
{
    /// <summary>
    /// 博客文章
    /// </summary>
    public class BlogPost : BaseEntity
    {

        private ICollection<BlogComment> _blogComments;

        /// <summary>
        /// 获取或设置对应语言ID
        /// </summary>
        public long LanguageId { get; set; }

        /// <summary>
        /// 获取或设置所在类目ID
        /// </summary>
        public long BlogCategoryId { get; set; }

        /// <summary>
        /// 获取或设置作者ID
        /// </summary>
        public long AuthorId { get; set; }

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 获取或设置正文内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 获取或设置概述
        /// </summary>
        public string BodyOverview { get; set; }

        /// <summary>
        /// 获取或设置标签
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// 获取或设置格式ID
        /// </summary>
        public int FormatId { get; set; }

        /// <summary>
        /// 获取或设置是否允许评论标识
        /// </summary>
        public bool AllowComments { get; set; }

        /// <summary>
        /// 获取或设置展示开始的UTC时间
        /// </summary>
        public DateTime? StartDateUtc { get; set; }

        /// <summary>
        /// 获取或设置展示结束的UTC时间
        /// </summary>
        public DateTime? EndDateUtc { get; set; }

        /// <summary>
        /// 获取或设置最后编辑结束的UTC时间
        /// </summary>
        public DateTime LastEditorDateUtc { get; set; }

        #region Navigation Properties

        /// <summary>
        /// 获取或设置博文格式
        /// </summary>
        public BlogPostFormat Format
        {
            get
            {
                return (BlogPostFormat)this.FormatId;
            }
            set { this.FormatId = (int)value; }
        }

        /// <summary>
        /// 获取或设置博文语言
        /// </summary>
        public virtual Language Language { get; set; }

        /// <summary>
        /// 获取或设置博文作者
        /// </summary>
        public virtual Customer Author { get; set; }

        /// <summary>
        /// 获取或设置博文类目
        /// </summary>
        public virtual BlogCategory BlogCategory { get; set; }

        /// <summary>
        /// 获取或设置博文内评论列表
        /// </summary>
        public virtual ICollection<BlogComment> BlogComments
        {
            get { return _blogComments ?? (_blogComments = new List<BlogComment>()); }
            set { _blogComments = value; }
        }

        #endregion

    }

}
