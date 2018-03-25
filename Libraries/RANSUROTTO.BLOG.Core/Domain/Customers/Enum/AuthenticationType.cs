namespace RANSUROTTO.BLOG.Core.Domain.Customers.Enum
{

    /// <summary>
    /// 登录类型
    /// </summary>
    public enum AuthenticationType
    {

        /// <summary>
        /// 仅可以使用用户名进行登录
        /// </summary>
        Username,

        /// <summary>
        /// 仅可以使用Email进行登录
        /// </summary>
        Email,

        /// <summary>
        /// 用户名及Email均可以用作登录
        /// </summary>
        UsernameOrEmail

    }
}
