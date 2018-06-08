using System;
using AutoMapper;
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

namespace RANSUROTTO.BLOG.Admin.Infrastructure.Mapper
{
    public class AdminMapperConfiguration : IMapperConfiguration
    {
        public Action<IMapperConfigurationExpression> GetConfiguration()
        {
            Action<IMapperConfigurationExpression> action = cfg =>
            {

                #region Localization

                cfg.CreateMap<Language, LanguageModel>()
                    .ForMember(dest => dest.TimeStamp, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore())
                    .ForMember(dest => dest.Search, mo => mo.Ignore());
                cfg.CreateMap<LanguageModel, Language>()
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore())
                    .ForMember(dest => dest.LocaleStringResources, mo => mo.Ignore());

                #endregion

                #region Logging

                cfg.CreateMap<Log, LogModel>()
                    .ForMember(dest => dest.TimeStamp, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<LogModel, Log>()
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore());

                #endregion

                #region Messages

                cfg.CreateMap<EmailAccount, EmailAccountModel>()
                    .ForMember(dest => dest.TimeStamp, mo => mo.Ignore())
                    .ForMember(dest => dest.Password, mo => mo.Ignore())
                    .ForMember(dest => dest.IsDefaultEmailAccount, mo => mo.Ignore())
                    .ForMember(dest => dest.SendTestEmailTo, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<EmailAccountModel, EmailAccount>()
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore())
                    .ForMember(dest => dest.Password, mo => mo.Ignore());

                #endregion

                #region Customers

                cfg.CreateMap<EmailAccount, EmailAccountModel>()
                    .ForMember(dest => dest.TimeStamp, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<EmailAccountModel, EmailAccount>()
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore());

                #endregion

                #region Customer roles

                cfg.CreateMap<CustomerRole, CustomerRoleModel>()
                    .ForMember(dest => dest.TimeStamp, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CustomerRoleModel, CustomerRole>()
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore());

                #endregion

                #region Activity log types

                cfg.CreateMap<ActivityLogType, ActivityLogTypeModel>()
                    .ForMember(dest => dest.TimeStamp, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<ActivityLogTypeModel, ActivityLogType>()
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore());

                #endregion

                #region Activity logs

                cfg.CreateMap<ActivityLog, ActivityLogModel>()
                    .ForMember(dest => dest.ActivityLogTypeName, mo => mo.MapFrom(src => src.ActivityLogType.Name))
                    .ForMember(dest => dest.CustomerEmail, mo => mo.MapFrom(src => src.Customer.Email))
                    .ForMember(dest => dest.TimeStamp, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<ActivityLogTypeModel, ActivityLogType>()
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore());

                #endregion

                #region Blog categories

                cfg.CreateMap<Category, CategoryModel>()
                    .ForMember(dest => dest.TimeStamp, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CategoryModel, Category>()
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore());

                #endregion

                #region Ideas

                cfg.CreateMap<Idea, IdeaModel>()
                    .ForMember(dest => dest.TimeStamp, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<IdeaModel, Idea>()
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore());

                #endregion

                #region Blog posts

                cfg.CreateMap<BlogPost, BlogPostModel>()
                    .ForMember(dest => dest.TimeStamp, mo => mo.Ignore())
                    .ForMember(dest => dest.Deleted, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore())
                    .ForMember(dest => dest.BlogPostTags, mo => mo.Ignore());
                cfg.CreateMap<BlogPostModel, BlogPost>()
                    .ForMember(dest => dest.AuthorId, mo => mo.Ignore())
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore())
                    .ForMember(dest => dest.UpdatedOnUtc, mo => mo.Ignore())
                    .ForMember(dest => dest.BlogPostTags, mo => mo.Ignore());

                #endregion

                #region Blog tags

                cfg.CreateMap<BlogPostTag, BlogPostTagModel>()
                    .ForMember(dest => dest.TimeStamp, mo => mo.Ignore())
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<BlogPostTagModel, BlogPostTag>()
                    .ForMember(dest => dest.CreatedOnUtc, mo => mo.Ignore());

                #endregion

                #region Settings

                cfg.CreateMap<CustomerSettings, CustomerUserSettingsModel.CustomerSettingsModel>()
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<CustomerUserSettingsModel.CustomerSettingsModel, CustomerSettings>();

                cfg.CreateMap<BlogSettings, BlogSettingsModel>()
                    .ForMember(dest => dest.CustomProperties, mo => mo.Ignore());
                cfg.CreateMap<BlogSettingsModel, BlogSettings>();

                #endregion

            };
            return action;
        }

        public int Order => 0;

    }
}