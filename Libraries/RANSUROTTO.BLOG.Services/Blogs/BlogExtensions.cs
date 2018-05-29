using System;
using System.Linq;
using RANSUROTTO.BLOG.Core.Domain.Blogs;

namespace RANSUROTTO.BLOG.Services.Blogs
{
    public static class BlogExtensions
    {

        public static bool BlogPostTagExists(this BlogPost blogPost, int blogPostTagId)
        {
            if (blogPost == null)
                throw new ArgumentNullException(nameof(blogPost));

            bool result = blogPost.BlogPostTags.ToList().Find(pt => pt.Id == blogPostTagId) != null;
            return result;
        }

    }
}
