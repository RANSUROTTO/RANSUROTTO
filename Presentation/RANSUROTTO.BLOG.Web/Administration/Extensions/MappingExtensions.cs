using RANSUROTTO.BLOG.Admin.Models.Blogs;
using RANSUROTTO.BLOG.Admin.Models.Customers;
using RANSUROTTO.BLOG.Admin.Models.Interesting;
using RANSUROTTO.BLOG.Admin.Models.Localization;
using RANSUROTTO.BLOG.Admin.Models.Logging;
using RANSUROTTO.BLOG.Admin.Models.Messages;
using RANSUROTTO.BLOG.Admin.Models.Settings;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Blogs.Setting;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Core.Domain.Interesting;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Domain.Logging;
using RANSUROTTO.BLOG.Core.Domain.Messages;
using RANSUROTTO.BLOG.Core.Infrastructure.Mapper;

namespace RANSUROTTO.BLOG.Admin.Extensions
{
    public static class MappingExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
        {
            return AutoMapperConfiguration.Mapper.Map<TSource, TDestination>(source);
        }

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
        {
            return AutoMapperConfiguration.Mapper.Map(source, destination);
        }

        #region Localization

        public static LanguageModel ToModel(this Language entity)
        {
            return entity.MapTo<Language, LanguageModel>();
        }

        public static Language ToEntity(this LanguageModel model)
        {
            return model.MapTo<LanguageModel, Language>();
        }

        public static Language ToEntity(this LanguageModel model, Language destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Messages

        public static EmailAccountModel ToModel(this EmailAccount entity)
        {
            return entity.MapTo<EmailAccount, EmailAccountModel>();
        }

        public static EmailAccount ToEntity(this EmailAccountModel model)
        {
            return model.MapTo<EmailAccountModel, EmailAccount>();
        }

        public static EmailAccount ToEntity(this EmailAccountModel model, EmailAccount destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Customer role

        public static CustomerRoleModel ToModel(this CustomerRole entity)
        {
            return entity.MapTo<CustomerRole, CustomerRoleModel>();
        }

        public static CustomerRole ToEntity(this CustomerRoleModel model)
        {
            return model.MapTo<CustomerRoleModel, CustomerRole>();
        }

        public static CustomerRole ToEntity(this CustomerRoleModel model, CustomerRole destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Activity log type

        public static ActivityLogTypeModel ToModel(this ActivityLogType entity)
        {
            return entity.MapTo<ActivityLogType, ActivityLogTypeModel>();
        }

        public static ActivityLogType ToEntity(this ActivityLogTypeModel model)
        {
            return model.MapTo<ActivityLogTypeModel, ActivityLogType>();
        }

        public static ActivityLogType ToEntity(this ActivityLogTypeModel model, ActivityLogType destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Activity log

        public static ActivityLogModel ToModel(this ActivityLog entity)
        {
            return entity.MapTo<ActivityLog, ActivityLogModel>();
        }

        public static ActivityLog ToEntity(this ActivityLogModel model)
        {
            return model.MapTo<ActivityLogModel, ActivityLog>();
        }

        public static ActivityLog ToEntity(this ActivityLogModel model, ActivityLog destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Blog categories

        public static CategoryModel ToModel(this Category entity)
        {
            return entity.MapTo<Category, CategoryModel>();
        }

        public static Category ToEntity(this CategoryModel model)
        {
            return model.MapTo<CategoryModel, Category>();
        }

        public static Category ToEntity(this CategoryModel model, Category destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Ideas

        public static IdeaModel ToModel(this Idea entity)
        {
            return entity.MapTo<Idea, IdeaModel>();
        }

        public static Idea ToEntity(this IdeaModel model)
        {
            return model.MapTo<IdeaModel, Idea>();
        }

        public static Idea ToEntity(this IdeaModel model, Idea destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Blog posts

        public static BlogPostModel ToModel(this BlogPost entity)
        {
            return entity.MapTo<BlogPost, BlogPostModel>();
        }

        public static BlogPost ToEntity(this BlogPostModel model)
        {
            return model.MapTo<BlogPostModel, BlogPost>();
        }

        public static BlogPost ToEntity(this BlogPostModel model, BlogPost destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Blog tags

        public static BlogPostTagModel ToModel(this BlogPostTag entity)
        {
            return entity.MapTo<BlogPostTag, BlogPostTagModel>();
        }

        public static BlogPostTag ToEntity(this BlogPostTagModel model)
        {
            return model.MapTo<BlogPostTagModel, BlogPostTag>();
        }

        public static BlogPostTag ToEntity(this BlogPostTagModel model, BlogPostTag destination)
        {
            return model.MapTo(destination);
        }

        #endregion

        #region Settings

        public static CustomerUserSettingsModel.CustomerSettingsModel ToModel(this CustomerSettings entity)
        {
            return entity.MapTo<CustomerSettings, CustomerUserSettingsModel.CustomerSettingsModel>();
        }

        public static CustomerSettings ToEntity(this CustomerUserSettingsModel.CustomerSettingsModel model)
        {
            return model.MapTo<CustomerUserSettingsModel.CustomerSettingsModel, CustomerSettings>();
        }

        public static CustomerSettings ToEntity(this CustomerUserSettingsModel.CustomerSettingsModel model, CustomerSettings destination)
        {
            return model.MapTo(destination);
        }

        public static BlogSettingsModel ToModel(this BlogSettings entity)
        {
            return entity.MapTo<BlogSettings, BlogSettingsModel>();
        }

        public static BlogSettings ToEntity(this BlogSettingsModel model)
        {
            return model.MapTo<BlogSettingsModel, BlogSettings>();
        }

        public static BlogSettings ToEntity(this BlogSettingsModel model, BlogSettings destination)
        {
            return model.MapTo(destination);
        }

        #endregion

    }
}