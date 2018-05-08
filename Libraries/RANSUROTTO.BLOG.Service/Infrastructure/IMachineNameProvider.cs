namespace RANSUROTTO.BLOG.Services.Infrastructure
{

    /// <summary>
    /// 描述运行应用程序的机器（实例）的名称的服务
    /// </summary>
    public interface IMachineNameProvider
    {
        /// <summary>
        /// 返回运行应用程序的机器（实例）的名称
        /// </summary>
        string GetMachineName();
    }
}
