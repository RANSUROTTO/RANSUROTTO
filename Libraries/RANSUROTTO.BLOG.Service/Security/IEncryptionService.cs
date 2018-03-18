namespace RANSUROTTO.BLOG.Service.Security
{

    public interface IEncryptionService
    {

        /// <summary>
        /// �����μ�
        /// </summary>
        /// <param name="size">����С</param>
        /// <returns>�μ�</returns>
        string CreateSaltKey(int size);

        /// <summary>
        /// ��������ɢ�й�ϣ
        /// </summary>
        /// <param name="password">����</param>
        /// <param name="saltkey">�μ�</param>
        /// <param name="passwordFormat">�����ʽ(��ϣ�㷨)</param>
        /// <returns>�����ϣֵ</returns>
        string CreatePasswordHash(string password, string saltkey, string passwordFormat = "SHA1");

        /// <summary>
        /// ��������ɢ�й�ϣ
        /// </summary>
        /// <param name="data">��ɢ�м��������</param>
        /// <param name="hashAlgorithm">��ϣ�㷨</param>
        /// <returns>���ݹ�ϣֵ</returns>
        string CreateHash(byte[] data, string hashAlgorithm = "SHA1");

        /// <summary>
        /// �����ı�
        /// </summary>
        /// <param name="plainText">����ܵ��ı�</param>
        /// <param name="encryptionPrivateKey">������Կ</param>
        /// <returns>���ܺ���ı�</returns>
        string EncryptText(string plainText, string encryptionPrivateKey = "");

        /// <summary>
        /// �����ı�
        /// </summary>
        /// <param name="plainText">����ܵ��ı�</param>
        /// <param name="encryptionPrivateKey">������Կ</param>
        /// <returns>���ܺ���ı�</returns>
        string DecryptText(string plainText, string encryptionPrivateKey = "");

    }

}
