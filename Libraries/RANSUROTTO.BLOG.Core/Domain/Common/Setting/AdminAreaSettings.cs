using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Common.Setting
{
    public class AdminAreaSettings : ISettings
    {

        /// <summary>
        /// 获取或设置DataGrid默认每页显示的记录数
        /// </summary>
        public int DefaultGridPageSize { get; set; }

        /// <summary>
        /// 获取或设置弹窗层中的DataGrid默认每页显示的记录数
        /// </summary>
        public int PopupGridPageSize { get; set; }

        /// <summary>
        /// 获取或设置网格每页显示的记录数可选列表用逗号分割的字符串
        /// </summary>
        public string GridPageSizes { get; set; }

        /// <summary>
        /// 获取或设置是否使用ISO时间格式来处理JSON内的时间属性
        /// true: yyyy-MM-dd HH:mm:ss //China ISO8601
        /// false:\/Date(xxxxxxxxxx)\/
        /// </summary>
        public bool UseIsoDateTimeConverterInJson { get; set; }

    }
}
