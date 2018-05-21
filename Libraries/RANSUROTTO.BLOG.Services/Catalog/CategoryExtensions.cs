using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RANSUROTTO.BLOG.Core.Domain.Blogs;

namespace RANSUROTTO.BLOG.Services.Catalog
{
    public static class CategoryExtensions
    {

        /// <summary>
        /// 获取以树的方式排序类目
        /// </summary>
        /// <param name="source">类目数据源</param>
        /// <param name="parentId">父类目标识符</param>
        /// <param name="ignoreCategoriesWithoutExistingParent">指示是否忽略没有父分类的类目</param>
        /// <returns>排序后的类目</returns>
        public static IList<BlogCategory> SortBlogCategoriesForTree(this IList<BlogCategory> source, long parentId = 0,
            bool ignoreCategoriesWithoutExistingParent = false)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            var result = new List<BlogCategory>();

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



    }
}
