using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Media;

namespace RANSUROTTO.BLOG.Core.Domain.Interesting
{
    /// <summary>
    /// 想法
    /// </summary>
    public class Idea : BaseEntity
    {

        private ICollection<Picture> _pictures;

        /// <summary>
        /// 获取或设置想法内容
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// 获取或设置该想法是否已被删除
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// 获取或设置保密设置(不公开/私密)
        /// </summary>
        public bool Private { get; set; }

        public int CustomerId { get; set; }

        #region Navigation properies

        public virtual Customer Customer { get; set; }

        public virtual ICollection<Picture> Pictures
        {
            get { return _pictures ?? (_pictures = new List<Picture>()); }
            set { _pictures = value; }
        }

        #endregion

    }
}
