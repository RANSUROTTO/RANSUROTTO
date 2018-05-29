using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Services.Catalog;

namespace RANSUROTTO.BLOG.Admin.Helpers
{
    /// <summary>
    /// 选择列表帮助类
    /// </summary>
    public static class SelectListHelper
    {

        /// <summary>
        /// 获取博客类目选择列表
        /// </summary>
        /// <param name="categoryService">类目业务实例</param>
        /// <param name="cacheManager">缓存管理实例</param>
        /// <param name="showHidden">显示隐藏的类目</param>
        /// <returns>博客类目选择列表</returns>
        public static List<SelectListItem> GetBlogCategoryList(ICategoryService categoryService, ICacheManager cacheManager,
            bool showHidden = false)
        {
            if (categoryService == null)
                throw new ArgumentNullException(nameof(categoryService));

            if (cacheManager == null)
                throw new ArgumentNullException(nameof(cacheManager));

            string cacheKey = string.Format("Ransurotto.pres.admin.categories.list-{0}", showHidden);
            var listItems = cacheManager.Get(cacheKey, () =>
            {
                var categories = categoryService.GetAllCategories(showHidden: showHidden);
                return categories.Select(c => new SelectListItem
                {
                    Text = c.GetFormattedBreadCrumb(categories),
                    Value = c.Id.ToString()
                });
            });

            var result = new List<SelectListItem>();
            //克隆列表以确保未设置“选定”属性
            foreach (var item in listItems)
            {
                result.Add(new SelectListItem
                {
                    Text = item.Text,
                    Value = item.Value
                });
            }
            return result;
        }

    }
}