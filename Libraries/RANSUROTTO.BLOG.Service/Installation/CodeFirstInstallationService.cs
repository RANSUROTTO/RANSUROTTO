using System;
using System.Collections.Generic;
using System.Linq;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Blog;
using RANSUROTTO.BLOG.Core.Domain.Blog.Setting;
using RANSUROTTO.BLOG.Core.Domain.Common;
using RANSUROTTO.BLOG.Core.Domain.Common.Setting;
using RANSUROTTO.BLOG.Core.Domain.Configuration;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Customers.Enum;
using RANSUROTTO.BLOG.Core.Domain.Customers.Service;
using RANSUROTTO.BLOG.Core.Domain.Customers.Setting;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Domain.Localization.Setting;
using RANSUROTTO.BLOG.Core.Domain.Logging;
using RANSUROTTO.BLOG.Core.Domain.Logging.Setting;
using RANSUROTTO.BLOG.Core.Domain.Security;
using RANSUROTTO.BLOG.Core.Domain.Security.Setting;
using RANSUROTTO.BLOG.Core.Domain.Seo.Enum;
using RANSUROTTO.BLOG.Core.Domain.Seo.Setting;
using RANSUROTTO.BLOG.Core.Domain.Tasks;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Service.Configuration;
using RANSUROTTO.BLOG.Service.Customers;
using RANSUROTTO.BLOG.Service.Helpers.Setting;

namespace RANSUROTTO.BLOG.Service.Installation
{
    public class CodeFirstInstallationService : IInstallationService
    {

        #region Fields

        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IRepository<BlogComment> _blogCommentRepository;
        private readonly IRepository<GenericAttribute> _genericAttributeRepository;
        private readonly IRepository<Setting> _settingRepository;
        private readonly IRepository<Language> _languageRepository;
        private readonly IRepository<LocaleStringResource> _localeStringResourceRepository;
        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IRepository<ActivityLog> _activityLogRepository;
        private readonly IRepository<Log> _logRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<CustomerPassword> _customerPasswordRepository;
        private readonly IRepository<ScheduleTask> _scheduleTaskRepository;
        private readonly IRepository<CustomerRole> _customerRoleRepository;
        private readonly IRepository<PermissionRecord> _permissionRecordRepository;

        #endregion

        #region Constructor

        public CodeFirstInstallationService(IRepository<Category> categoryRepository, IRepository<BlogPost> blogPostRepository, IRepository<BlogComment> blogCommentRepository, IRepository<GenericAttribute> genericAttributeRepository, IRepository<Setting> settingRepository, IRepository<Language> languageRepository, IRepository<LocaleStringResource> localeStringResourceRepository, IRepository<ActivityLogType> activityLogTypeRepository, IRepository<ActivityLog> activityLogRepository, IRepository<Log> logRepository, IRepository<Customer> customerRepository, IRepository<CustomerPassword> customerPasswordRepository, IRepository<ScheduleTask> scheduleTaskRepository, IRepository<CustomerRole> customerRoleRepository, IRepository<PermissionRecord> permissionRecordRepository)
        {
            _categoryRepository = categoryRepository;
            _blogPostRepository = blogPostRepository;
            _blogCommentRepository = blogCommentRepository;
            _genericAttributeRepository = genericAttributeRepository;
            _settingRepository = settingRepository;
            _languageRepository = languageRepository;
            _localeStringResourceRepository = localeStringResourceRepository;
            _activityLogTypeRepository = activityLogTypeRepository;
            _activityLogRepository = activityLogRepository;
            _logRepository = logRepository;
            _customerRepository = customerRepository;
            _customerPasswordRepository = customerPasswordRepository;
            _scheduleTaskRepository = scheduleTaskRepository;
            _customerRoleRepository = customerRoleRepository;
            _permissionRecordRepository = permissionRecordRepository;
        }

        #endregion

        #region Methods

        public void InstallData(string defaultUserEmail, string defaultUserPassword, bool installSampleData = true)
        {
            InstallLanguages();
            InstallCustomersAndUsers(defaultUserEmail, defaultUserPassword);
            InstallSettings(installSampleData);
            InstallLocaleResources();
        }

        #endregion

        #region Utilities

        protected virtual void InstallLanguages()
        {
            var englishLanguage = new Language
            {
                Name = "English",
                LanguageCulture = "en-US",
                UniqueSeoCode = "en",
                //FlagImageFileName = "us.png",
                Published = true,
                DisplayOrder = 3
            };
            _languageRepository.Insert(englishLanguage);

            var japanLanguage = new Language
            {
                Name = "日本語",
                LanguageCulture = "ja",
                UniqueSeoCode = "ja",
                //FlagImageFileName = "ja.png",
                Published = true,
                DisplayOrder = 2
            };
            _languageRepository.Insert(japanLanguage);


            var chinaLanguage = new Language
            {
                Name = "中文简体",
                LanguageCulture = "zh-CN",
                UniqueSeoCode = "zh",
                //FlagImageFileName = "cn.png",
                Published = true,
                DisplayOrder = 1
            };
            _languageRepository.Insert(chinaLanguage);
        }

        protected virtual void InstallLocaleResources()
        {


        }

        protected virtual void InstallCustomersAndUsers(string defaultUserEmail, string defaultUserPassword)
        {
            var crAdministrators = new CustomerRole
            {
                Name = "Administrators",
                Active = true,
                IsSystemRole = true,
                SystemName = SystemCustomerRoleNames.Administrators,
            };
            var crRegistered = new CustomerRole
            {
                Name = "Registered",
                Active = true,
                IsSystemRole = true,
                SystemName = SystemCustomerRoleNames.Registered,
            };
            var crGuests = new CustomerRole
            {
                Name = "Guests",
                Active = true,
                IsSystemRole = true,
                SystemName = SystemCustomerRoleNames.Guests,
            };
            var customerRoles = new List<CustomerRole>
            {
                crAdministrators,
                crRegistered,
                crGuests
            };
            _customerRoleRepository.Insert(customerRoles);

            var adminUser = new Customer
            {
                Email = defaultUserEmail,
                Username = "Administrator",
                Active = true,
                LastActivityDateUtc = DateTime.UtcNow
            };

            adminUser.CustomerRoles.Add(crRegistered);
            adminUser.CustomerRoles.Add(crAdministrators);

            _customerRepository.Insert(adminUser);

            var customerRegistrationService = EngineContext.Current.Resolve<ICustomerRegistrationService>();
            customerRegistrationService.ChangePassword(new ChangePasswordRequest(defaultUserEmail, false,
                PasswordFormat.Hashed, defaultUserPassword));

            //搜索引擎（内置）用户
            var searchEngineUser = new Customer
            {
                Username = "search Engine User",
                Email = "builtin@search_engine_record.com",
                Guid = Guid.NewGuid(),
                AdminComment = "内置用户,用于处理搜索引擎的请求.",
                Active = true,
                IsSystemAccount = true,
                SystemName = SystemCustomerNames.SearchEngine,
                LastActivityDateUtc = DateTime.UtcNow
            };
            searchEngineUser.CustomerRoles.Add(crGuests);
            _customerRepository.Insert(searchEngineUser);

            var backgroundTaskUser = new Customer
            {
                Username = "background Task User",
                Email = "builtin@background-task-record.com",
                AdminComment = "内置用户,用于处理后台任务.",
                Active = true,
                IsSystemAccount = true,
                SystemName = SystemCustomerNames.BackgroundTask,
                LastActivityDateUtc = DateTime.UtcNow
            };
            backgroundTaskUser.CustomerRoles.Add(crGuests);
            _customerRepository.Insert(backgroundTaskUser);

        }

        protected virtual void InstallSettings(bool installSampleData)
        {
            var settingService = EngineContext.Current.Resolve<ISettingService>();
            settingService.SaveSetting(new CommonSettings
            {
                DisplayEuCookieLawWarning = true,
                DisplayJavaScriptDisabledWarning = true,
                DefaultTheme = null,
                Log404Errors = false,
                RenderXuaCompatible = true,
                XuaCompatibleValue = "<meta http-equiv=\"X-UA-Compatible\" content=\"IE=edge,chrome=1\" />"
            });

            settingService.SaveSetting(new SeoSettings
            {
                PageTitleSeparator = ". ",
                PageTitleSeoAdjustment = PageTitleSeoAdjustment.PagenameAfterBlogname,
                DefaultTitle = "Your Blog",
                DefaultMetaKeywords = "",
                DefaultMetaDescription = "",
                CustomHeadTags = "",
                EnableJsBundling = false,
                EnableCssBundling = false,
                WwwRequirement = WwwRequirement.NoMatter
            });

            settingService.SaveSetting(new AdminAreaSettings
            {
                DefaultGridPageSize = 15,
                PopupGridPageSize = 10,
                GridPageSizes = "10, 15, 20, 50, 100",
                UseIsoDateTimeConverterInJson = true
            });

            settingService.SaveSetting(new LocalizationSettings
            {
                DefaultAdminLanguageId = _languageRepository.Table.Single(l => l.Name == "English").Id,
                UseImagesForLanguageSelection = false,
                SeoFriendlyUrlsForLanguagesEnabled = false,
                AutomaticallyDetectLanguage = false,
                LoadAllLocaleRecordsOnStartup = true,
                LoadAllLocalizedPropertiesOnStartup = true,
                LoadAllUrlRecordsOnStartup = false,
                IgnoreRtlPropertyForAdminArea = false
            });

            settingService.SaveSetting(new SecuritySettings
            {
                SslEnabled = false,
                ForceSslForAllPages = false,
                EncryptionKey = CommonHelper.GenerateRandomDigitCode(16),
                AdminAreaAllowedIpAddresses = new List<string>(),
                EnableXsrfProtectionForAdminArea = true,
                EnableXsrfProtectionForPublicArea = true
            });

            settingService.SaveSetting(new CustomerSettings
            {
                CurrentAuthenticationType = AuthenticationType.UsernameOrEmail,
                HashedPasswordFormat = "SHA1",
                FailedPasswordAllowedAttempts = 5,
                FailedPasswordLockoutMinutes = 10,
                UnduplicatedPasswordsNumber = 0
            });

            settingService.SaveSetting(new DateTimeSettings
            {
                DefaultStoreTimeZoneId = "",
                AllowCustomersToSetTimeZone = false
            });

            settingService.SaveSetting(new BlogSettings
            {
                AllowNotRegisteredUserToLeaveComments = false,
                MaxNumberOfTags = 15,
                BlogCommentsMustBeApproved = false
            });

            settingService.SaveSetting(new LogSettings
            {
                IgnoreSoftDelete = false,
                IgnoreLogWordlist = new List<string>()
            });

        }

        protected virtual void InstallActivityLogTypes()
        {
            var activityLogTypes = new List<ActivityLogType>
            {
                new ActivityLogType
                {
                    SystemKeyword = "AddNewCategory",
                    Enabled = true,
                    Name = "Add a new category"
                }
            };
            _activityLogTypeRepository.Insert(activityLogTypes);
        }

        protected virtual void InstallScheduleTasks()
        {

        }

        #endregion

    }
}
