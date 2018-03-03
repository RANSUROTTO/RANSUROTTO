using System.Collections.Generic;

namespace RANSUROTTO.BLOG.Core.Common
{

    /// <summary>
    /// 分页集合接口
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {

        /// <summary>
        /// 当前页页码
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// 每页展示的项数量
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 查询结果项总数量
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// 查询结果的总页数
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// 标识是否存在上一页
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// 标识是否存在下一页
        /// </summary>
        bool HasNextPage { get; }

    }

}
