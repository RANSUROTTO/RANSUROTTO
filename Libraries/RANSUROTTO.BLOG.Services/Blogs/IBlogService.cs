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
        /// true:只查询已发布的
        /// false:查询未发布或已结束发布的
        /// </param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        IPagedList<BlogPost> GetAllBlogPosts(int pageIndex = 0, int pageSize = int.MaxValue,
            IList<int> categoryIds = null, IList<int> customerIds = null, IList<int> tagIds = null,
            string keywords = null, bool? overridePublished = true, BlogSortingEnum orderBy = BlogSortingEnum.Position);

        BlogPost GetBlogPostById(int blogPostId);

        void InsertBlogPost(BlogPost blogPost);

        void UpdateBlogPost(BlogPost blogPost);

        void DeleteBlogPost(BlogPost blogPost);

        #endregion

        #region Blog comments



        #endregion

    }
}
