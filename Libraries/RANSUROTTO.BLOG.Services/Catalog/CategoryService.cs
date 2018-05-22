using System;
using System.Collections.Generic;
using System.Linq;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Common.Setting;
using RANSUROTTO.BLOG.Services.Events;

namespace RANSUROTTO.BLOG.Services.Catalog
{
    public class CategoryService : ICategoryService
    {

        #region Constants

        /// <summary>
        /// 博客类目缓存
        /// </summary>
        /// <remarks>
        /// {0} : 博客类目标识符
        /// </remarks>
        private const string BLOGCATEGORIES_BY_ID_KEY = "Ransurotto.category.id-{0}";

        /// <summary>
        /// 博客类目缓存
        /// </summary>
        /// <remarks>
        /// {0} : 父类目标识符
        /// {1} : 显示隐藏记录?
        /// {2} : 加载所有子级?
        /// </remarks>
        private const string BLOGCATEGORIES_BY_PARENT_CATEGORY_ID_KEY = "Ransurotto.category.byparent-{0}-{1}-{2}";

        /// <summary>
        /// 清除博客类目缓存的键匹配模式
        /// </summary>
        private const string BLOGCATEGORIES_PATTERN_KEY = "Ransurotto.category.";

        #endregion

        #region Fields

        private readonly IRepository<BlogCategory> _blogCategoryRepository;
        private readonly IWorkContext _workContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly IDataProvider _dataProvider;
        private readonly CommonSettings _commonSettings;

        #endregion

        #region Constructor

        public CategoryService(IRepository<BlogCategory> blogCategoryRepository, IWorkContext workContext, IEventPublisher eventPublisher, ICacheManager cacheManager, IDataProvider dataProvider, CommonSettings commonSettings)
        {
            _blogCategoryRepository = blogCategoryRepository;
            _workContext = workContext;
            _eventPublisher = eventPublisher;
            _cacheManager = cacheManager;
            _dataProvider = dataProvider;
            _commonSettings = commonSettings;
        }

        #endregion

        #region Methods

        public IPagedList<BlogCategory> GetAllBlogCategories(string blogCategoryName = "", int pageIndex = 0, int pageSize = Int32.MaxValue,
            bool showHidden = false)
        {
            if (_commonSettings.UseStoredProcedureForLoadingCategories &&
                _commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
            {
                throw new NotImplementedException();
            }
            else
            {
                //不支持存储过程、采用LINQ
                var query = _blogCategoryRepository.Table;
                if (!showHidden)
                    query = query.Where(c => c.Published);
                if (!string.IsNullOrWhiteSpace(blogCategoryName))
                    query = query.Where(c => c.Name.Contains(blogCategoryName));
                query = query.OrderBy(c => c.ParentCategoryId).ThenBy(c => c.DisplayOrder).ThenBy(c => c.Id);

                var unsortedBlogCategories = query.ToList();

                var sortedBlogCategories = unsortedBlogCategories.SortBlogCategoriesForTree();

                return new PagedList<BlogCategory>(sortedBlogCategories, pageIndex, pageSize);
            }
        }

        public IList<BlogCategory> GetAllBlogCategoriesByParentCategoryId(long parentBlogCategoryId, bool showHidden = false,
            bool includeAllLevels = false)
        {
            string key = string.Format(BLOGCATEGORIES_BY_PARENT_CATEGORY_ID_KEY, parentBlogCategoryId, showHidden, includeAllLevels);
            return _cacheManager.Get(key, () =>
            {
                var query = _blogCategoryRepository.Table;
                if (!showHidden)
                    query = query.Where(c => c.Published);
                query = query.Where(c => c.ParentCategoryId == parentBlogCategoryId);
                query = query.Where(c => !c.Deleted);
                query = query.OrderBy(c => c.DisplayOrder).ThenBy(c => c.Id);

                var categories = query.ToList();
                if (includeAllLevels)
                {
                    //加载所有子级类目
                    var childCategories = new List<BlogCategory>();
                    foreach (var category in categories)
                    {
                        childCategories.AddRange(GetAllBlogCategoriesByParentCategoryId(category.Id, showHidden, true));
                    }
                    categories.AddRange(childCategories);
                }

                return categories;
            });
        }

        public BlogCategory GetBlogCategoryById(int blogCategoryId)
        {
            if (blogCategoryId == 0)
                return null;

            string key = string.Format(BLOGCATEGORIES_BY_ID_KEY, blogCategoryId);
            return _cacheManager.Get(key, () => _blogCategoryRepository.GetById(blogCategoryId));
        }

        public void InsertBlogCategory(BlogCategory blogCategory)
        {
            if (blogCategory == null)
                throw new ArgumentNullException(nameof(blogCategory));

            _blogCategoryRepository.Insert(blogCategory);

            //cache
            _cacheManager.RemoveByPattern(BLOGCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(blogCategory);
        }

        public void UpdateBlogCategory(BlogCategory blogCategory)
        {
            if (blogCategory == null)
                throw new ArgumentNullException(nameof(blogCategory));

            //验证类目层次
            var parentCategory = GetBlogCategoryById(blogCategory.ParentCategoryId);
            while (parentCategory != null)
            {
                if (blogCategory.Id == parentCategory.Id)
                {
                    blogCategory.ParentCategoryId = 0;
                    break;
                }
                parentCategory = GetBlogCategoryById(parentCategory.ParentCategoryId);
            }

            _blogCategoryRepository.Update(blogCategory);

            //cache
            _cacheManager.RemoveByPattern(BLOGCATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(blogCategory);
        }

        public void DeleteBlogCategory(BlogCategory blogCategory)
        {
            if (blogCategory == null)
                throw new ArgumentNullException(nameof(blogCategory));

            blogCategory.Deleted = true;
            UpdateBlogCategory(blogCategory);

            _eventPublisher.EntityDeleted(blogCategory);

            var subcategories = GetAllBlogCategoriesByParentCategoryId(blogCategory.Id, true);
            foreach (var subcategory in subcategories)
            {
                //将其子类目清除父类目标识
                subcategory.ParentCategoryId = 0;
                UpdateBlogCategory(subcategory);
            }
        }

        #endregion

    }
}
