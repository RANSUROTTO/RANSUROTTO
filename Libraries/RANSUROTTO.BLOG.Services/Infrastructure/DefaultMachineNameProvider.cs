namespace RANSUROTTO.BLOG.Services.Infrastructure
{
    /// <summary>
    /// 默认机器名提供者
    /// </summary>
    public class DefaultMachineNameProvider : IMachineNameProvider
    {
        /// <summary>
        /// 返回运行应用程序的机器（实例）的名称
        /// </summary>
        public string GetMachineName()
        {
            return System.Environment.MachineName;
        }
    }
}
