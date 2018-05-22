using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;

namespace RANSUROTTO.BLOG.Core.Domain.Customers
{

    /// <summary>
    /// �û�����
    /// </summary>
    public class CustomerPassword : BaseEntity
    {

        /// <summary>
        /// ��ȡ�������û�ID
        /// </summary>
        public int CustomerId { get; set; }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// ��ȡ�����������ʽ������ID
        /// </summary>
        public int PasswordFormatId { get; set; }

        /// <summary>
        /// ��ȡ������������
        /// </summary>
        public string PasswordSalt { get; set; }

        #region Navigation Properties

        /// <summary>
        /// ��ȡ�����������ʽ������
        /// </summary>
        public PasswordFormat PasswordFormat
        {
            get { return (PasswordFormat)this.PasswordFormatId; }
            set { PasswordFormatId = (int)value; }
        }

        /// <summary>
        /// ��ȡ�������û�
        /// </summary>
        public virtual Customer Customer { get; set; }

        #endregion

    }

}
