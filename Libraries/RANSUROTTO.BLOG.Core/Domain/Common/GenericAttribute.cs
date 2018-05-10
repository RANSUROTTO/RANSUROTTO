using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Common
{
    /// <summary>
    /// 通用自定义实体属性
    /// </summary>
    public class GenericAttribute : BaseEntity
    {

        /// <summary>
        /// 获取或设置所属实体的标识符
        /// </summary>
        public long EntityId { get; set; }

        /// <summary>
        /// 获取或设置键分组
        /// </summary>
        public string KeyGroup { get; set; }

        /// <summary>
        /// 获取或设置键
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public string Value { get; set; }

    }
}
