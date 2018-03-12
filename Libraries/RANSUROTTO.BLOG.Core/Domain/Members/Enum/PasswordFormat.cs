namespace RANSUROTTO.BLOG.Core.Domain.Members.Enum
{

    /// <summary>
    /// 密码处理格式
    /// </summary>
    public enum PasswordFormat
    {

        /// <summary>
        /// 空/不进行格式处理
        /// </summary>
        Clear,

        /// <summary>
        /// 使用哈希散列算法格式处理
        /// </summary>
        Hashed,

        /// <summary>
        /// 使用对称加密算法格式处理
        /// </summary>
        Encrypted,

    }

}
