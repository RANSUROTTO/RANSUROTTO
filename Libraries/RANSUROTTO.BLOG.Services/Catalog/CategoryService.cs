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
        /// 博客对应类目列表缓存
        /// </summary>
        /// <remarks>
        /// {0} : 显示隐藏记录?
        /// {1} : 博客文章标识符
        /// {2} : 当前用户标识符
        /// </remarks>
        private const string BLOGPOSTCATEGORIES_ALLBYPRODUCTID_KEY = "Ransurotto.blogpostcategory.allbyblogpostid-{0}-{1}-{2}";

        /// <summary>
        /// 清除博客类目缓存的键匹配模式
        /// </summary>
        private const string CATEGORIES_PATTERN_KEY = "Ransurotto.category.";

        /// <summary>
        /// 清除博客文章关联类目缓存的键匹配模式
        /// </summary>
        private const string BLOGPOSTCATEGORIES_PATTERN_KEY = "Ransurotto.blogpostcategory.";

        #endregion

        #region Fields

        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IRepository<Category> _blogCategoryRepository;
        private readonly IRepository<BlogPostCategory> _blogPostBlogCategoryRepository;
        private readonly IWorkContext _workContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;
        private readonly IDataProvider _dataProvider;
        private readonly CommonSettings _commonSettings;

        #endregion

        #region Constructor

        public CategoryService(IRepository<BlogPost> blogPostRepository, IRepository<Category> blogCategoryRepository, IRepository<BlogPostCategory> blogPostBlogCategoryRepository, IWorkContext workContext, IEventPublisher eventPublisher, ICacheManager cacheManager, IDataProvider dataProvider, CommonSettings commonSettings)
        {
            _blogPostRepository = blogPostRepository;
            _blogCategoryRepository = blogCategoryRepository;
            _blogPostBlogCategoryRepository = blogPostBlogCategoryRepository;
            _workContext = workContext;
            _eventPublisher = eventPublisher;
            _cacheManager = cacheManager;
            _dataProvider = dataProvider;
            _commonSettings = commonSettings;
        }

        #endregion

        #region Methods

        public virtual IPagedList<Category> GetAllCategories(string blogCategoryName = "", int pageIndex = 0, int pageSize = Int32.MaxValue,
            bool showHidden = false)
        {
            if (_commonSettings.UseStoredProcedureForLoadingCategories &&
                _commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
            {
                //TODO 没有实现存储过程
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

                return new PagedList<Category>(sortedBlogCategories, pageIndex, pageSize);
            }
        }

        public virtual IList<Category> GetAllCategoriesByParentCategoryId(int parentBlogCategoryId, bool showHidden = false,
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
                    var childCategories = new List<Category>();
                    foreach (var category in categories)
                    {
                        childCategories.AddRange(GetAllCategoriesByParentCategoryId(category.Id, showHidden, true));
                    }
                    categories.AddRange(childCategories);
                }

                return categories;
            });
        }

        public virtual Category GetCategoryById(int blogCategoryId)
        {
            if (blogCategoryId == 0)
                return null;

            string key = string.Format(BLOGCATEGORIES_BY_ID_KEY, blogCategoryId);
            return _cacheManager.Get(key, () => _blogCategoryRepository.GetById(blogCategoryId));
        }

        public virtual void InsertCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            _blogCategoryRepository.Insert(category);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(category);
        }

        public virtual void UpdateCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            //验证类目层次
            var parentCategory = GetCategoryById(category.ParentCategoryId);
            while (parentCategory != null)
            {
                if (category.Id == parentCategory.Id)
                {
                    category.ParentCategoryId = 0;
                    break;
                }
                parentCategory = GetCategoryById(parentCategory.ParentCategoryId);
            }

            _blogCategoryRepository.Update(category);

            //cache
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(category);
        }

        public virtual void DeleteCategory(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            category.Deleted = true;
            UpdateCategory(category);

            _eventPublisher.EntityDeleted(category);

            var subcategories = GetAllCategoriesByParentCategoryId(category.Id, true);
            foreach (var subcategory in subcategories)
            {
                //将其子类目清除父类目标识
                subcategory.ParentCategoryId = 0;
                UpdateCategory(subcategory);
            }
        }

        public virtual IList<BlogPostCategory> GetCategoriesByBlogPostId(int blogPostId, bool showHidden = false)
        {
            if (blogPostId == 0)
                return new List<BlogPostCategory>();

            string key = string.Format(BLOGPOSTCATEGORIES_ALLBYPRODUCTID_KEY, showHidden, blogPostId, _workContext.CurrentCustomer.Id);
            return _cacheManager.Get(key, () =>
            {
                var query = from bc in _blogPostBlogCategoryRepository.Table
                            join c in _blogCategoryRepository.Table on bc.BlogCategoryId equals c.Id
                            where bc.BlogCategoryId == blogPostId &&
                                  !c.Deleted &&
                                  (showHidden || c.Published)
                            orderby bc.DisplayOrder, bc.Id
                            select bc;

                var allBlogPostCategories = query.ToList();
                return allBlogPostCategories;
            });
        }

        public virtual void InsertBlogPostCategory(BlogPostCategory blogPostCategory)
        {
            if (blogPostCategory == null)
                throw new ArgumentNullException(nameof(blogPostCategory));

            _blogPostBlogCategoryRepository.Insert(blogPostCategory);

            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(BLOGPOSTCATEGORIES_PATTERN_KEY);

            _eventPublisher.EntityInserted(blogPostCategory);
        }

        public virtual void DeleteBlogPostCategory(BlogPostCategory blogPostCategory)
        {
            if (blogPostCategory == null)
                throw new ArgumentException(nameof(blogPostCategory));

            _blogPostBlogCategoryRepository.Delete(blogPostCategory);

            _cacheManager.RemoveByPattern(BLOGPOSTCATEGORIES_PATTERN_KEY);
            _cacheManager.RemoveByPattern(CATEGORIES_PATTERN_KEY);

            _eventPublisher.EntityDeleted(blogPostCategory);
        }

        #endregion

    }
}
