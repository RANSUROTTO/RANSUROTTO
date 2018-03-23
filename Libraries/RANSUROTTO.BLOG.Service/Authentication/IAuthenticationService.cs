using RANSUROTTO.BLOG.Core.Domain.Customers;

namespace RANSUROTTO.BLOG.Service.Authentication
{

    /// <summary>
    /// 身份认证业务层接口
    /// </summary>
    public interface IAuthenticationService
    {

        /// <summary>
        /// 签名认证
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="createPersistentCookie">指示是否创建持久性Cookie</param>
        void SignIn(Customer customer, bool createPersistentCookie);

        /// <summary>
        /// 取消签名认证
        /// </summary>
        void SignOut();

        /// <summary>
        /// 获取已签名认证的用户
        /// </summary>
        /// <returns>用户</returns>
        Customer GetAuthenticatedCustomer();

    }

}
