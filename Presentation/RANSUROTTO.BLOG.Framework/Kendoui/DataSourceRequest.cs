namespace RANSUROTTO.BLOG.Framework.Kendoui
{

    /// <summary>
    /// KendoUi DataGrid 请求数据封装类
    /// </summary>
    public class DataSourceRequest
    {

        /// <summary>
        /// 请求页码（ >= 1 ）
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 请求页大小
        /// </summary>
        public int PageSize { get; set; }

        public DataSourceRequest()
        {
            Page = 1;
            PageSize = 10;
        }

    }

}
