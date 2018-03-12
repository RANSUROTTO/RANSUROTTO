using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Logging.Enum;
using RANSUROTTO.BLOG.Core.Domain.Members;

namespace RANSUROTTO.BLOG.Core.Domain.Logging
{

    public class Log : BaseEntity
    {

        /// <summary>
        /// ��ȡ��������־�ȼ�ID
        /// </summary>
        public int LogLevelId { get; set; }

        /// <summary>
        /// ��ȡ�����ö���Ϣ
        /// </summary>
        public string ShortMessage { get; set; }

        /// <summary>
        /// ��ȡ������������Ϣ
        /// </summary>
        public string FullMessage { get; set; }

        /// <summary>
        /// ��ȡ������IP��ַ
        /// </summary>
        public string IpAddress { get; set; }

        /// <summary>
        /// ��ȡ�������û�ID
        /// </summary>
        public long? CustomerId { get; set; }

        /// <summary>
        /// ��ȡ����������Url
        /// </summary>
        public string PageUrl { get; set; }

        /// <summary>
        /// ��ȡ����������Url
        /// </summary>
        public string ReferrerUrl { get; set; }

        #region Navigation Properties

        /// <summary>
        /// ��ȡ��������־�ȼ�
        /// </summary>
        public LogLevel LogLevel
        {
            get
            {
                return (LogLevel)LogLevelId;
            }
            set
            {
                LogLevelId = (int)value;
            }
        }

        /// <summary>
        /// ��ȡ�������û�
        /// </summary>
        public virtual Customer Customer { get; set; }

        #endregion

    }

}
