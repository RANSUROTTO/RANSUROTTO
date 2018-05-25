using System;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Blogs.Enum;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Core.Domain.Blogs
{
    /// <summary>
    /// ��������
    /// </summary>
    public class BlogPost : BaseEntity, ILocalizedEntity
    {

        private ICollection<BlogPostTag> _blogPostTags;
        private ICollection<BlogComment> _blogComments;
        private ICollection<BlogCategory> _blogCategories;

        /// <summary>
        /// ��ȡ����������ID
        /// </summary>
        public int AuthorId { get; set; }

        /// <summary>
        /// ��ȡ�����ñ���
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// ��ȡ��������������
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// ��ȡ�����ø���
        /// </summary>
        public string BodyOverview { get; set; }

        /// <summary>
        /// ��ȡ�����ø�ʽID
        /// </summary>
        public int FormatId { get; set; }

        /// <summary>
        /// ��ȡ�������Ƿ��������۱�ʶ
        /// </summary>
        public bool AllowComments { get; set; }

        /// <summary>
        /// ��ȡ������չʾ��ʼ��UTCʱ��
        /// </summary>
        public DateTime? AvailableStartDateUtc { get; set; }

        /// <summary>
        /// ��ȡ������չʾ������UTCʱ��
        /// </summary>
        public DateTime? AvailableEndDateUtc { get; set; }

        /// <summary>
        /// ��ȡ���������༭������UTCʱ��
        /// </summary>
        public DateTime UpdateOnUtc { get; set; }

        /// <summary>
        /// ��ȡ�����ø������Ƿ��ѱ�����ɾ��
        /// </summary>
        public bool Deleted { get; set; }

        #region Navigation Properties

        /// <summary>
        /// ��ȡ�����ò��ĸ�ʽ
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
        /// ��ȡ�����ò�������
        /// </summary>
        public virtual Customer Author { get; set; }

        /// <summary>
        /// ��ȡ�����ö�Ӧ����Ŀ�б�
        /// </summary>
        public virtual ICollection<BlogCategory> BlogCategories
        {
            get { return _blogCategories ?? (_blogCategories = new List<BlogCategory>()); }
            set { _blogCategories = value; }
        }

        /// <summary>
        /// ��ȡ�����ù�����ǩ�б�
        /// </summary>
        public virtual ICollection<BlogPostTag> BlogPostTags
        {
            get { return _blogPostTags ?? (_blogPostTags = new List<BlogPostTag>()); }
            set { _blogPostTags = value; }
        }

        /// <summary>
        /// ��ȡ�����ò����������б�
        /// </summary>
        public virtual ICollection<BlogComment> BlogComments
        {
            get { return _blogComments ?? (_blogComments = new List<BlogComment>()); }
            set { _blogComments = value; }
        }

        #endregion

    }

}
