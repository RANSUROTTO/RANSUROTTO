namespace RANSUROTTO.BLOG.Core.Domain.Seo.Enum
{
    /// <summary>
    /// www是否必须的
    /// </summary>
    public enum WwwRequirement
    {
        /// <summary>
        /// 无所谓
        /// </summary>
        NoMatter = 0,

        /// <summary>
        /// 应该要有www
        /// </summary>
        WithWww = 10,

        /// <summary>
        /// 不应该要有www
        /// </summary>
        WithoutWww = 20,
    }
}
