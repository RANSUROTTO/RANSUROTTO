namespace RANSUROTTO.BLOG.Core.Domain.Customers.Service
{
    /// <summary>
    /// 用户登录结果
    /// </summary>
    public enum CustomerLoginResults
    {

        /// <summary>
        /// 成功
        /// </summary>
        Successful = 1,

        /// <summary>
        /// 用户不存在
        /// </summary>
        CustomerNotExist = 2,

        /// <summary>
        /// 密码错误
        /// </summary>
        WrongPassword = 3,

        /// <summary>
        /// 被禁用/不可用
        /// </summary>
        NotActive = 4,

        /// <summary>
        /// 没有注册
        /// </summary>
        NotRegistered = 5,

        /// <summary>
        /// 被锁定
        /// </summary>
        LockedOut = 6

    }
}
