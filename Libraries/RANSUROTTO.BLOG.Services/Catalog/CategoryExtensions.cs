using System;
using System.Linq;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Services.Catalog
{
    public static class CategoryExtensions
    {

        /// <summary>
        /// 获取类目面包屑格式化后的字符串
        /// </summary>
        /// <param name="category">类目</param>
        /// <param name="categoryService">类目业务实例</param>
        /// <param name="separator">分隔符</param>
        /// <param name="languageId">语言标识符</param>
        /// <returns>格式化后的字符串</returns>
        public static string GetFormattedBreadCrumb(this Category category,
            ICategoryService categoryService,
            string separator = ">>", int languageId = 0)
        {
            string result = string.Empty;

            var breadcrumb = GetCategoryBreadCrumb(category, categoryService, true);
            for (int i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var categoryName = breadcrumb[i].GetLocalized(x => x.Name, languageId);
                result = String.IsNullOrEmpty(result)
                    ? categoryName
                    : string.Format("{0} {1} {2}", result, separator, categoryName);
            }

            return result;
        }

        /// <summary>
        /// 获取以树的方式排序类目
        /// </summary>
        /// <param name="source">类目数据源</param>
        /// <param name="parentId">父类目标识符</param>
        /// <param name="ignoreCategoriesWithoutExistingParent">指示是否忽略没有父分类的类目</param>
        /// <returns>排序后的类目</returns>
        public static IList<Category> SortBlogCategoriesForTree(this IList<Category> source, long parentId = 0,
            bool ignoreCategoriesWithoutExistingParent = false)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var result = new List<Category>();

            foreach (var cat in source.Where(c => c.ParentCategoryId == parentId).ToList())
            {
                result.Add(cat);
                result.AddRange(SortBlogCategoriesForTree(source, cat.Id, true));
            }
            if (!ignoreCategoriesWithoutExistingParent && result.Count != source.Count)
            {
                //将提供的类目数据源中查找没有父类目的类别,并将其插入结果中
                foreach (var cat in source)
                    if (result.FirstOrDefault(x => x.Id == cat.Id) == null)
                        result.Add(cat);
            }

            return result;
        }

        /// <summary>
        /// 获取面包屑格式化类目
        /// </summary>
        /// <param name="category">类目</param>
        /// <param name="allCategories">需被格式化操作的类目列表</param>
        /// <param name="separator">分隔符</param>
        /// <param name="languageId">语言标识符</param>
        /// <returns>格式化后的类目列表</returns>
        public static string GetFormattedBreadCrumb(this Category category,
            IList<Category> allCategories,
            string separator = ">>", int languageId = 0)
        {
            string result = string.Empty;

            var breadcrumb = GetCategoryBreadCrumb(category, allCategories, true);
            for (int i = 0; i <= breadcrumb.Count - 1; i++)
            {
                var categoryName = breadcrumb[i].GetLocalized(x => x.Name, languageId);
                result = string.IsNullOrEmpty(result)
                    ? categoryName
                    : string.Format("{0} {1} {2}", result, separator, categoryName);
            }

            return result;
        }

        /// <summary>
        /// 获取面包屑格式化类目
        /// </summary>
        /// <param name="category">类目</param>
        /// <param name="categoryService">类目业务实例</param>
        /// <param name="showHidden">指示是否显示隐藏项</param>
        /// <returns>格式化后的类目列表</returns>
        public static IList<Category> GetCategoryBreadCrumb(this Category category,
            ICategoryService categoryService,
            bool showHidden = false)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var result = new List<Category>();

            //用于防止循环引用
            var alreadyProcessedCategoryIds = new List<long>();

            while (category != null && //不为null
                   !category.Deleted && //不为已被删除
                   (showHidden || category.Published) && //已发布
                   !alreadyProcessedCategoryIds.Contains(category.Id)) //防止循环引用
            {
                result.Add(category);

                alreadyProcessedCategoryIds.Add(category.Id);

                category = categoryService.GetCategoryById(category.ParentCategoryId);
            }
            result.Reverse();
            return result;
        }

        /// <summary>
        /// 获取面包屑格式化类目
        /// </summary>
        /// <param name="category">类目</param>
        /// <param name="allCategories">需被格式化操作的类目列表</param>
        /// <param name="showHidden">指示是否显示隐藏项</param>
        /// <returns>格式化后的类目列表</returns>
        public static IList<Category> GetCategoryBreadCrumb(this Category category,
            IList<Category> allCategories,
            bool showHidden = false)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var result = new List<Category>();

            //用于防止循环引用
            var alreadyProcessedCategoryIds = new List<long>();

            while (category != null && //不为null
                   !category.Deleted && //不为已被删除
                   (showHidden || category.Published) && //已发布
                   !alreadyProcessedCategoryIds.Contains(category.Id)) //防止循环引用
            {
                result.Add(category);

                alreadyProcessedCategoryIds.Add(category.Id);

                category = (from c in allCategories
                            where c.Id == category.ParentCategoryId
                            select c).FirstOrDefault();
            }
            result.Reverse();
            return result;
        }

        public static BlogPostCategory FindBlogPostCategory(this IList<BlogPostCategory> source,
            int blogPostId, int categoryId)
        {
            foreach (var blogPostBlogCategory in source)
                if (blogPostBlogCategory.BlogPostId == blogPostId && blogPostBlogCategory.BlogCategoryId == categoryId)
                    return blogPostBlogCategory;

            return null;
        }

    }
}
