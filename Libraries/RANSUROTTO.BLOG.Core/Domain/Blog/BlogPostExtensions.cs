using System;
using System.Collections.Generic;

namespace RANSUROTTO.BLOG.Core.Domain.Blog
{

    public static class BlogPostExtensions
    {

        /// <summary>
        /// 将Tag属性转换为字符串数组
        /// </summary>
        /// <param name="blogPost">博文对象</param>
        /// <returns>标签数组</returns>
        public static string[] ParseTags(this BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException(nameof(blogPost));

            var parsedTags = new List<string>();
            if (!string.IsNullOrEmpty(blogPost.Tag))
            {
                string[] tags = blogPost.Tag.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var tag in tags)
                {
                    var tmp = tag.Trim();
                    if (!string.IsNullOrEmpty(tmp))
                        parsedTags.Add(tmp);
                }
            }
            return parsedTags.ToArray();
        }

    }

}
