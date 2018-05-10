using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Blog
{
    /// <summary>
    /// 博客类目
    /// </summary>
    public class Category : BaseEntity
    {

        /// <summary>
        /// 获取或设置类目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置发布状态
        /// </summary>
        public bool Publisher { get; set; }

        /// <summary>
        /// 获取或设置顺序
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// 获取或设置管理员赋予的备注
        /// </summary>
        public string AdminComment { get; set; }

    }
}
