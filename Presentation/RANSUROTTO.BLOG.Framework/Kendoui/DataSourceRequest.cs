namespace RANSUROTTO.BLOG.Framework.Kendoui
{

    /// <summary>
    /// KendoUi DataGrid �������ݷ�װ��
    /// </summary>
    public class DataSourceRequest
    {

        /// <summary>
        /// ����ҳ�루 >= 1 ��
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// ����ҳ��С
        /// </summary>
        public int PageSize { get; set; }

        public DataSourceRequest()
        {
            Page = 1;
            PageSize = 10;
        }

    }

}
