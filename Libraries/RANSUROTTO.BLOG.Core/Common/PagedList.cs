using System;
using System.Collections.Generic;
using System.Linq;

namespace RANSUROTTO.BLOG.Core.Common
{

    /// <summary>
    /// 分页集合
    /// </summary>
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T>
    {

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">查询源</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的项数</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int total = source.Count();
            this.TotalCount = total;
            this.TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">查询源</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的项数</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">查询源</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每页显示的项数</param>
        /// <param name="totalCount">查询结果总条数</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;
            this.AddRange(source);
        }

        public int PageIndex { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }

    }
}
