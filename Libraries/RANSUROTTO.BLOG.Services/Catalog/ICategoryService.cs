using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Domain.Blogs;

namespace RANSUROTTO.BLOG.Services.Catalog
{
    public interface ICategoryService
    {

        /// <summary>
        /// 获取博客类目列表
        /// </summary>
        /// <param name="blogCategoryName">类目名称</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">页大小</param>
        /// <param name="showHidden">指示是否显示隐藏的类目</param>
        /// <returns>博客类目列表</returns>
        IPagedList<BlogCategory> GetAllBlogCategories(string blogCategoryName = "",
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// 获取博客类目列表
        /// </summary>
        /// <param name="parentBlogCategoryId">父类目标识符</param>
        /// <param name="showHidden">指示是否显示隐藏的类目</param>
        /// <param name="includeAllLevels">指示是否加载所有联级子级类目</param>
        /// <returns>博客类目列表</returns>
        IList<BlogCategory> GetAllBlogCategoriesByParentCategoryId(long parentBlogCategoryId,
            bool showHidden = false, bool includeAllLevels = false);

        /// <summary>
        /// 通过标识符获取博客类目
        /// </summary>
        /// <param name="blogCategoryId">博客类目标识符</param>
        /// <returns>博客类目</returns>
        BlogCategory GetBlogCategoryById(long blogCategoryId);

        /// <summary>
        /// 添加博客类目
        /// </summary>
        /// <param name="blogCategory">博客类目</param>
        void InsertBlogCategory(BlogCategory blogCategory);

        /// <summary>
        /// 更新博客类目
        /// </summary>
        /// <param name="blogCategory">博客类目</param>
        void UpdateBlogCategory(BlogCategory blogCategory);

        /// <summary>
        /// 删除博客类目
        /// </summary>
        /// <param name="blogCategory">博客类目</param>
        void DeleteBlogCategory(BlogCategory blogCategory);

    }
}
