using RANSUROTTO.BLOG.Admin.Models.Blogs;
using RANSUROTTO.BLOG.Admin.Models.Customers;
using RANSUROTTO.BLOG.Admin.Models.Localization;
using RANSUROTTO.BLOG.Admin.Models.Logging;
using RANSUROTTO.BLOG.Admin.Models.Messages;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Customers;
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

        public static BlogCategoryModel ToModel(this BlogCategory entity)
        {
            return entity.MapTo<BlogCategory, BlogCategoryModel>();
        }

        public static BlogCategory ToEntity(this BlogCategoryModel model)
        {
            return model.MapTo<BlogCategoryModel, BlogCategory>();
        }

        public static BlogCategory ToEntity(this BlogCategoryModel model, BlogCategory destination)
        {
            return model.MapTo(destination);
        }

        #endregion

    }
}