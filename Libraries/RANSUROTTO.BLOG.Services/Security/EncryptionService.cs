using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using RANSUROTTO.BLOG.Core.Domain.Security.Setting;

namespace RANSUROTTO.BLOG.Services.Security
{
    public class EncryptionService : IEncryptionService
    {

        #region Fields

        private readonly SecuritySettings _securitySettings;

        #endregion

        #region Constructor

        public EncryptionService(SecuritySettings securitySettings)
        {
            _securitySettings = securitySettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 创建盐键
        /// </summary>
        /// <param name="size">盐键大小</param>
        /// <returns>盐键</returns>
        public virtual string CreateSaltKey(int size)
        {
            //生成加密随机数
            using (var provider = new RNGCryptoServiceProvider())
            {
                var buff = new byte[size];
                provider.GetBytes(buff);

                //返回一个随机数的Base64字符串表示形式
                return Convert.ToBase64String(buff);
            }
        }

        /// <summary>
        /// 创建密码散列哈希
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="saltkey">盐键</param>
        /// <param name="passwordFormat">密码格式（哈希散列）</param>
        /// <returns>密码哈希值</returns>
        public virtual string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1")
        {
            return CreateHash(Encoding.UTF8.GetBytes(String.Concat(password, saltkey)), passwordFormat);
        }

        /// <summary>
        /// 创建数据散列哈希
        /// </summary>
        /// <param name="data">需散列计算的数据</param>
        /// <param name="hashAlgorithm">哈希算法</param>
        /// <returns>数据哈希值</returns>
        public virtual string CreateHash(byte[] data, string hashAlgorithm = "SHA1")
        {
            if (String.IsNullOrEmpty(hashAlgorithm))
                hashAlgorithm = "SHA1";

            var algorithm = HashAlgorithm.Create(hashAlgorithm);
            if (algorithm == null)
                throw new ArgumentException("不存在的哈希散列方法！");

            var hashByteArray = algorithm.ComputeHash(data);
            return BitConverter.ToString(hashByteArray).Replace("-", "");
        }

        /// <summary>
        /// 加密文本
        /// </summary>
        /// <param name="plainText">需加密的文本</param>
        /// <param name="encryptionPrivateKey">加密密钥</param>
        /// <returns>加密后的文本</returns>
        public virtual string EncryptText(string plainText, string encryptionPrivateKey = "")
        {
            if (string.IsNullOrEmpty(plainText))
                return plainText;

            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = _securitySettings.EncryptionKey;

            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));

                byte[] encryptedBinary = EncryptTextToMemory(plainText, provider.Key, provider.IV);
                return Convert.ToBase64String(encryptedBinary);
            }
        }

        /// <summary>
        /// 解密文本
        /// </summary>
        /// <param name="cipherText">需解密的文本</param>
        /// <param name="encryptionPrivateKey">解密密钥</param>
        /// <returns>解密后的文本</returns>
        public virtual string DecryptText(string cipherText, string encryptionPrivateKey = "")
        {
            if (String.IsNullOrEmpty(cipherText))
                return cipherText;

            if (String.IsNullOrEmpty(encryptionPrivateKey))
                encryptionPrivateKey = _securitySettings.EncryptionKey;

            using (var provider = new TripleDESCryptoServiceProvider())
            {
                provider.Key = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(0, 16));
                provider.IV = Encoding.ASCII.GetBytes(encryptionPrivateKey.Substring(8, 8));

                byte[] buffer = Convert.FromBase64String(cipherText);
                return DecryptTextFromMemory(buffer, provider.Key, provider.IV);
            }
        }

        #endregion

        #region Utilities

        private byte[] EncryptTextToMemory(string data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    byte[] toEncrypt = Encoding.Unicode.GetBytes(data);
                    cs.Write(toEncrypt, 0, toEncrypt.Length);
                    cs.FlushFinalBlock();
                }

                return ms.ToArray();
            }
        }

        private string DecryptTextFromMemory(byte[] data, byte[] key, byte[] iv)
        {
            using (var ms = new MemoryStream(data))
            {
                using (var cs = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(key, iv), CryptoStreamMode.Read))
                {
                    using (var sr = new StreamReader(cs, Encoding.Unicode))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

        #endregion

    }
}
