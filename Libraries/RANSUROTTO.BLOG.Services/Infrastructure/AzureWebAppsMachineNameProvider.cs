
using System;

namespace RANSUROTTO.BLOG.Services.Infrastructure
{
    /// <summary>
    /// Auzre Web应用程序机器名称提供程序
    /// </summary>
    public class AzureWebAppsMachineNameProvider : IMachineNameProvider
    {
        /// <summary>
        /// 返回运行应用程序的机器（实例）的名称
        /// </summary>
        public string GetMachineName()
        {
            //如果在Windows Azure云服务（Web角色）上运行，请使用下面的代码
            //return Microsoft.WindowsAzure.ServiceRuntime.RoleEnvironment.CurrentRoleInstance.Id;

            var name = Environment.GetEnvironmentVariable("WEBSITE_INSTANCE_ID");
            if (String.IsNullOrEmpty(name))
                name = Environment.MachineName;

            return name;
        }
    }
}
