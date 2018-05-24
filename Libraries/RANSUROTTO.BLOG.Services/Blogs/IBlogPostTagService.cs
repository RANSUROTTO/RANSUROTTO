using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Blogs;

namespace RANSUROTTO.BLOG.Services.Blogs
{
    public interface IBlogPostTagService
    {

        IList<BlogPostTag> GetAllBlogPostTags();

        BlogPostTag GetBlogPostTagById(int blogPostTagId);

        BlogPostTag GetBlogPostTagByName(string name);

        void InsertBlogPostTag(BlogPostTag blogPostTag);

        void UpdateBlogPostTag(BlogPostTag blogPostTag);

        void DeleteBlogPostTag(BlogPostTag blogPostTag);

        int GetBlogPostCount(int blogPostTagId);

    }
}
