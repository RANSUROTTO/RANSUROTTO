using System;
using System.Linq;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Customers
{

    /// <summary>
    /// �û�
    /// </summary>
    public class Customer : BaseEntity
    {

        private ICollection<CustomerRole> _customerRoles;

        /// <summary>
        /// ��ȡ�������û���/��¼��
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// ��ȡ�����õ�������
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// ��ȡ�����õ��������Ƿ���Ҫ�ٴ���֤
        /// </summary>
        public bool EmailToRevalidate { get; set; }

        /// <summary>
        /// ��ȡ������ϵͳ����
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// ��ȡ�����ñ�ʶ���û��Ƿ�Ϊϵͳ�˻�
        /// </summary>
        public bool IsSystemAccount { get; set; }

        /// <summary>
        /// ��ȡ�����ø��û����������Ե�½ʧ�ܴ��� (�������)
        /// </summary>
        public int FailedLoginAttempts { get; set; }

        /// <summary>
        /// ��ȡ�����ø��û���ָ��UTCʱ��֮ǰ�ܾ���½
        /// </summary>
        public DateTime? CannotLoginUntilDateUtc { get; set; }

        /// <summary>
        /// ��ȡ�����ø��û��Ƿ�Ϊ����״̬
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// ��ȡ������������/����ʱ��IP��ַ
        /// </summary>
        public string LastIpAddress { get; set; }

        /// <summary>
        /// ��ȡ������������/����ʱ��UTCʱ��
        /// </summary>
        public DateTime LastActivityDateUtc { get; set; }

        /// <summary>
        /// ��ȡ����������½ʱ��UTCʱ��
        /// </summary>
        public DateTime? LastLoginDateUtc { get; set; }

        /// <summary>
        /// ��ȡ�����ù���Ա�Ը��û���ע����Ϣ
        /// </summary>
        public string AdminComment { get; set; }

        #region Navigation Properties

        /// <summary>
        /// ��ȡ�������û�Ȩ�޽�ɫ
        /// </summary>
        public virtual ICollection<CustomerRole> CustomerRoles
        {
            get { return _customerRoles?.Where(p => !p.IsDeleted).ToList() ?? (_customerRoles = new List<CustomerRole>()); }
            protected set { _customerRoles = value; }
        }

        #endregion

    }

}
