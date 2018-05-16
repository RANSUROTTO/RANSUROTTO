using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;

namespace RANSUROTTO.BLOG.Services.Customers
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

        /// <summary>
        /// 更改密码
        /// </summary>
        /// <param name="request">请求模型</param>
        /// <returns>结果</returns>
        ChangePasswordResult ChangePassword(ChangePasswordRequest request);

        /// <summary>
        /// 设置新用户名
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="newUsername">新用户名</param>
        void SetUsername(Customer customer, string newUsername);

        /// <summary>
        /// 设置新电子邮箱
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="newEmail">新电子邮箱</param>
        /// <param name="requireValidation">需要发送邮件进行验证</param>
        void SetEmail(Customer customer, string newEmail, bool requireValidation);

    }
}
