using System.Collections.Generic;

namespace RANSUROTTO.BLOG.Core.Common
{

    /// <summary>
    /// ��ҳ���Ͻӿ�
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {

        /// <summary>
        /// ��ǰҳҳ��
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// ÿҳչʾ��������
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// ��ѯ�����������
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// ��ѯ�������ҳ��
        /// </summary>
        int TotalPages { get; }

        /// <summary>
        /// ��ʶ�Ƿ������һҳ
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// ��ʶ�Ƿ������һҳ
        /// </summary>
        bool HasNextPage { get; }

    }

}
