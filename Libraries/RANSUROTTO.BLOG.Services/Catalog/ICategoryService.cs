using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Domain.Blogs;

namespace RANSUROTTO.BLOG.Services.Catalog
{
    public interface ICategoryService
    {

        #region Categories

        /// <summary>
        /// 获取博客类目列表
        /// </summary>
        /// <param name="blogCategoryName">类目名称</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="showHidden">指示是否获取隐藏的类目</param>
        /// <returns>博客类目列表</returns>
        IPagedList<Category> GetAllCategories(string blogCategoryName = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// 获取博客类目列表
        /// </summary>
        /// <param name="parentBlogCategoryId">父类目标识符</param>
        /// <param name="showHidden">指示是否获取隐藏的类目</param>
        /// <param name="includeAllLevels">指示是否加载所有联级子级类目</param>
        /// <returns>博客类目列表</returns>
        IList<Category> GetAllCategoriesByParentCategoryId(int parentBlogCategoryId,
            bool showHidden = false, bool includeAllLevels = false);

        /// <summary>
        /// 通过标识符获取博客类目
        /// </summary>
        /// <param name="blogCategoryId">博客类目标识符</param>
        /// <returns>博客类目</returns>
        Category GetCategoryById(int blogCategoryId);

        /// <summary>
        /// 添加博客类目
        /// </summary>
        /// <param name="category">博客类目</param>
        void InsertCategory(Category category);

        /// <summary>
        /// 更新博客类目
        /// </summary>
        /// <param name="category">博客类目</param>
        void UpdateCategory(Category category);

        /// <summary>
        /// 删除博客类目
        /// </summary>
        /// <param name="category">博客类目</param>
        void DeleteCategory(Category category);

        #endregion

        #region Blog post category

        /// <summary>
        /// 获取博客文章引用的博客类目关联列表
        /// </summary>
        /// <param name="blogPostId">博客文章标识符</param>
        /// <param name="showHidden">指示是否获取隐藏的类目</param>
        /// <returns>博客类目列表</returns>
        IList<BlogPostCategory> GetCategoriesByBlogPostId(int blogPostId, bool showHidden = false);

        /// <summary>
        /// 添加博客文章和博客类目的关联对象
        /// </summary>
        /// <param name="blogPostCategory">博客文章和博客类目的关联对象</param>
        void InsertBlogPostCategory(BlogPostCategory blogPostCategory);

        /// <summary>
        /// 删除博客文章和博客类目的关联对象
        /// </summary>
        /// <param name="blogPostCategory">博客文章和博客类目的关联对象</param>
        void DeleteBlogPostCategory(BlogPostCategory blogPostCategory);

        #endregion

    }
}
