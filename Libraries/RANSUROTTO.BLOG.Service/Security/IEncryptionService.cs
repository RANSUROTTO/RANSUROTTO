namespace RANSUROTTO.BLOG.Service.Security
{

    public interface IEncryptionService
    {

        /// <summary>
        /// 创建盐键
        /// </summary>
        /// <param name="size">键大小</param>
        /// <returns>盐键</returns>
        string CreateSaltKey(int size);

        /// <summary>
        /// 创建密码散列哈希
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="saltkey">盐键</param>
        /// <param name="passwordFormat">密码格式(哈希算法)</param>
        /// <returns>密码哈希值</returns>
        string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1");

        /// <summary>
        /// 创建数据散列哈希
        /// </summary>
        /// <param name="data">需散列计算的数据</param>
        /// <param name="hashAlgorithm">哈希算法</param>
        /// <returns>数据哈希值</returns>
        string CreateHash(byte[] data, string hashAlgorithm = "SHA1");

        /// <summary>
        /// 加密文本
        /// </summary>
        /// <param name="plainText">需加密的文本</param>
        /// <param name="encryptionPrivateKey">加密密钥</param>
        /// <returns>加密后的文本</returns>
        string EncryptText(string plainText, string encryptionPrivateKey = "");

        /// <summary>
        /// 解密文本
        /// </summary>
        /// <param name="plainText">需解密的文本</param>
        /// <param name="encryptionPrivateKey">解密密钥</param>
        /// <returns>解密后的文本</returns>
        string DecryptText(string plainText, string encryptionPrivateKey = "");

    }

}
