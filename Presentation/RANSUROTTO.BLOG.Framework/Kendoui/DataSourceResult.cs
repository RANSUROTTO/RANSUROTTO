using System.Collections;

namespace RANSUROTTO.BLOG.Framework.Kendoui
{

    /// <summary>
    /// KendoUi DataGrid ��Ӧ���ݷ�װ��
    /// </summary>
    public class DataSourceResult
    {

        /// <summary>
        /// ��������
        /// </summary>
        public object ExtraData { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public IEnumerable Data { get; set; }

        /// <summary>
        /// �����б�
        /// </summary>
        public object Errors { get; set; }

        /// <summary>
        /// �ܼ�¼��
        /// </summary>
        public int Total { get; set; }

    }

}
