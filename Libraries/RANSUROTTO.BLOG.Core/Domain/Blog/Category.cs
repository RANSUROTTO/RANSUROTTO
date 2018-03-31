using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Blog
{
    /// <summary>
    /// 类目
    /// </summary>
    public class Category : BaseEntity
    {

        /// <summary>
        /// 获取或设置类目名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置可视状态
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// 获取或设置图标 font-awesome
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 获取或设置管理员赋予的备注
        /// </summary>
        public string AdminComment { get; set; }

    }
}
