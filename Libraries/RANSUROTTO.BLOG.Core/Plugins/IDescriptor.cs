namespace RANSUROTTO.BLOG.Core.Plugins
{

    /// <summary>
    /// 应用程序扩展（插件或主题）的描述信息接口
    /// </summary>
    public interface IDescriptor
    {

        /// <summary>
        /// 系统名称
        /// </summary>
        string SystemName { get; set; }

        /// <summary>
        /// 友好名称
        /// </summary>
        string FriendlyName { get; set; }

    }

}
