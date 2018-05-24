using System;
using System.Collections.Generic;
using System.Linq;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Common;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Blogs.Enum;
using RANSUROTTO.BLOG.Core.Domain.Common.Setting;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Services.Events;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Services.Blogs
{
    public class BlogService : IBlogService
    {

        #region Fields

        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly ILocalizationService _localizationService;
        private readonly IDataProvider _dataProvider;
        private readonly IEventPublisher _eventPublisher;
        private readonly CommonSettings _commonSettings;

        #endregion

        #region Constructor

        public BlogService(IRepository<BlogPost> blogPostRepository, IRepository<Customer> customerRepository, ILocalizationService localizationService, IDataProvider dataProvider, IEventPublisher eventPublisher, CommonSettings commonSettings)
        {
            _blogPostRepository = blogPostRepository;
            _customerRepository = customerRepository;
            _localizationService = localizationService;
            _dataProvider = dataProvider;
            _eventPublisher = eventPublisher;
            _commonSettings = commonSettings;
        }

        #endregion

        #region Blog posts

        public virtual IPagedList<BlogPost> GetAllBlogPosts(int pageIndex = 0, int pageSize = Int32.MaxValue,
            IList<int> categoryIds = null, IList<int> customerIds = null, IList<int> tagIds = null,
            string keywords = null, bool showHidden = false, BlogSortingEnum orderBy = BlogSortingEnum.Position)
        {
            //验证categoryIds和customerIds、tagIds
            if (categoryIds != null && categoryIds.Contains(0))
                categoryIds.Remove(0);
            if (customerIds != null && customerIds.Contains(0))
                customerIds.Remove(0);
            if (tagIds != null && tagIds.Contains(0))
                tagIds.Remove(0);

            if (_commonSettings.UseStoredProceduresIfSupported && _dataProvider.StoredProceduredSupported)
            {
                //如果应用程序开启了存储过程检索并且数据库提供程序支持存储过程则使用存储过程来进行查询
                //这比LINQ要更快、效率更高

                //TODO 没有实现存储过程
                throw new NotImplementedException();
            }
            else
            {
                #region Search blog posts

                var query = _blogPostRepository.Table;
                query = query.Where(q => !q.Deleted);

                var nowUtc = DateTime.UtcNow;

                if (!showHidden)
                {
                    query = query.Where(p =>
                        (!p.AvailableStartDateUtc.HasValue || p.AvailableStartDateUtc.Value < nowUtc) &&
                        (!p.AvailableEndDateUtc.HasValue || p.AvailableEndDateUtc.Value > nowUtc));
                }

                if (!string.IsNullOrWhiteSpace(keywords))
                {
                    query = from post in query
                            where post.Title.Contains(keywords)
                            select post;
                }

                if (categoryIds != null && categoryIds.Any())
                {
                    query = from post in query
                            from pc in post.BlogCategories.Where(pc => categoryIds.Contains(pc.Id))
                            select post;
                }

                if (customerIds != null && customerIds.Any())
                {
                    query = from post in query
                            join ct in _customerRepository.Table on post.AuthorId equals ct.Id into post_ct
                            from ct in post_ct.DefaultIfEmpty()
                            where customerIds.Contains(ct.Id)
                            select post;
                }

                if (tagIds != null && tagIds.Any())
                {
                    query = from post in query
                            from pt in post.BlogPostTags.Where(pt => tagIds.Contains(pt.Id))
                            select post;
                }

                switch (orderBy)
                {
                    case BlogSortingEnum.Position:
                        query = query.OrderBy(p => p.Title);
                        break;
                    case BlogSortingEnum.TitleAsc:
                        query = query.OrderBy(p => p.Title);
                        break;
                    case BlogSortingEnum.CreatedOn:
                        query = query.OrderByDescending(p => p.CreatedOnUtc);
                        break;
                    default:
                        query = query.OrderBy(p => p.Title);
                        break;
                }

                var blogs = new PagedList<BlogPost>(query, pageIndex, pageSize);

                return blogs;

                #endregion
            }
        }

        public virtual BlogPost GetBlogPostById(int blogPostId)
        {
            if (blogPostId == 0)
                return null;

            return _blogPostRepository.GetById(blogPostId);
        }

        public virtual void InsertBlogPost(BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException(nameof(blogPost));

            _blogPostRepository.Insert(blogPost);

            _eventPublisher.EntityInserted(blogPost);
        }

        public virtual void UpdateBlogPost(BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException(nameof(blogPost));

            _blogPostRepository.Update(blogPost);

            _eventPublisher.EntityUpdated(blogPost);
        }

        public virtual void DeleteBlogPost(BlogPost blogPost)
        {
            if (blogPost == null)
                throw new ArgumentNullException(nameof(blogPost));

            blogPost.Deleted = true;
            _blogPostRepository.Update(blogPost);

            _eventPublisher.EntityDeleted(blogPost);
        }

        #endregion

    }
}
