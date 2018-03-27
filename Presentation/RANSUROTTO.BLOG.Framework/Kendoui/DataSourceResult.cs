using System.Collections;

namespace RANSUROTTO.BLOG.Framework.Kendoui
{

    /// <summary>
    /// KendoUi DataGrid 相应数据封装类
    /// </summary>
    public class DataSourceResult
    {

        /// <summary>
        /// 额外数据
        /// </summary>
        public object ExtraData { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public IEnumerable Data { get; set; }

        /// <summary>
        /// 错误列表
        /// </summary>
        public object Errors { get; set; }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int Total { get; set; }

    }

}
