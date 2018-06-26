using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Blogs.Enum;

namespace RANSUROTTO.BLOG.Services.Blogs
{
    public interface IBlogService
    {

        #region Blog posts

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="categoryIds"></param>
        /// <param name="customerIds"></param>
        /// <param name="tagIds"></param>
        /// <param name="keywords"></param>
        /// <param name="overridePublished">
        /// null:全部
        /// true:只查询公开可见的
        /// false:查询未发布或已可见的
        /// </param>
        /// <param name="showDeleted">显示已删除的</param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IPagedList<BlogPost> GetAllBlogPosts(int pageIndex = 0, int pageSize = int.MaxValue,
            IList<int> categoryIds = null, IList<int> customerIds = null, IList<int> tagIds = null,
            string keywords = null, bool? overridePublished = true, bool? showDeleted = false, BlogSortingEnum orderBy = BlogSortingEnum.Position);

        BlogPost GetBlogPostById(int blogPostId);

        void InsertBlogPost(BlogPost blogPost);

        void UpdateBlogPost(BlogPost blogPost);

        void DeleteBlogPost(BlogPost blogPost);

        #endregion

        #region Blog comments



        #endregion

    }
}
