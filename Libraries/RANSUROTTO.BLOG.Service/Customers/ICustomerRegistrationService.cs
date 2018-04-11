using RANSUROTTO.BLOG.Core.Domain.Customers.Service;

namespace RANSUROTTO.BLOG.Service.Customers
{
    public interface ICustomerRegistrationService
    {

        /// <summary>
        /// 用户验证
        /// </summary>
        /// <param name="usernameOrEmail">用户名/Email</param>
        /// <param name="password">登录密码</param>
        /// <returns>验证结果</returns>
        CustomerLoginResults ValidateCustomer(string usernameOrEmail, string password);

    }
}
