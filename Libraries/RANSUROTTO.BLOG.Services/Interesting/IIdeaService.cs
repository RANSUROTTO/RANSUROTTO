using System;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Domain.Interesting;

namespace RANSUROTTO.BLOG.Services.Interesting
{
    public interface IIdeaService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="createFromUtc"></param>
        /// <param name="createToUtc"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Idea> GetAllIdeas(DateTime? createFromUtc = null, DateTime? createToUtc = null,
            int pageIndex = 0, int pageSize = int.MaxValue);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ideaId"></param>
        /// <returns></returns>
        Idea GetIdeaById(int ideaId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idea"></param>
        void InsertIdea(Idea idea);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idea"></param>
        void UpdateIdea(Idea idea);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="idea"></param>
        void DeleteIdea(Idea idea);

    }
}
