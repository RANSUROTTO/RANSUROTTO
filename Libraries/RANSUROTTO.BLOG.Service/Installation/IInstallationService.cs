namespace RANSUROTTO.BLOG.Services.Installation
{
    /// <summary>
    /// 安装服务业务层接口
    /// </summary>
    public interface IInstallationService
    {

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="defaultUserEmail">默认管理员邮箱</param>
        /// <param name="defaultUserPassword">默认管理员密码</param>
        /// <param name="installSampleData">插入测试数据</param>
        void InstallData(string defaultUserEmail, string defaultUserPassword, bool installSampleData = true);

    }
}
