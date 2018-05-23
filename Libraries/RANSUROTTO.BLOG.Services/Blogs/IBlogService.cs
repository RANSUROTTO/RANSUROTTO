using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Blogs.Enum;

namespace RANSUROTTO.BLOG.Services.Blogs
{
    public interface IBlogService
    {

        #region Blog posts

        IPagedList<BlogPost> GetAllBlogPosts(int pageIndex = 0, int pageSize = int.MaxValue,
            IList<int> categoryIds = null, IList<int> customerIds = null, IList<int> tagIds = null,
            string keywords = null, bool showHidden = false, BlogSortingEnum orderBy = BlogSortingEnum.Position);

        BlogPost GetBlogPostById(int blogPostId);

        void InsertBlogPost(BlogPost blogPost);

        void UpdateBlogPost(BlogPost blogPost);

        void DeleteBlogPost(BlogPost blogPost);

        #endregion

        #region Blog comments



        #endregion

        #region Blog tags

        IList<BlogPostTag> GetAllBlogPostTags();

        BlogPostTag GetBlogPostTagById(int blogPostTagId);

        BlogPostTag GetBlogPostTagByName(string name);

        void InsertBlogPostTag(BlogPostTag blogPostTag);

        void UpdateBlogPostTag(BlogPostTag blogPostTag);

        void DeleteBlogPostTag(BlogPostTag blogPostTag);

        int GetBlogCountByTag(int blogPostTagId);

        #endregion

    }
}
