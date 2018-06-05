using System;
using System.Collections.Generic;
using System.Linq;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Blogs;
using RANSUROTTO.BLOG.Core.Domain.Blogs.Setting;
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
using RANSUROTTO.BLOG.Core.Domain.Media.Setting;
using RANSUROTTO.BLOG.Core.Domain.Messages;
using RANSUROTTO.BLOG.Core.Domain.Security;
using RANSUROTTO.BLOG.Core.Domain.Security.Setting;
using RANSUROTTO.BLOG.Core.Domain.Seo.Enum;
using RANSUROTTO.BLOG.Core.Domain.Seo.Setting;
using RANSUROTTO.BLOG.Core.Domain.Tasks;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Services.Configuration;
using RANSUROTTO.BLOG.Services.Customers;
using RANSUROTTO.BLOG.Services.Helpers.Setting;

namespace RANSUROTTO.BLOG.Services.Installation
{
    public class CodeFirstInstallationService : IInstallationService
    {

        #region Fields

        private readonly IRepository<Category> _blogCategoryRepository;
        private readonly IRepository<BlogPost> _blogPostRepository;
        private readonly IRepository<Comment> _blogCommentRepository;
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
        private readonly IRepository<EmailAccount> _emailAccountRepository;

        #endregion

        #region Constructor

        public CodeFirstInstallationService(IRepository<Category> blogCategoryRepository, IRepository<BlogPost> blogPostRepository, IRepository<Comment> blogCommentRepository, IRepository<GenericAttribute> genericAttributeRepository, IRepository<Setting> settingRepository, IRepository<Language> languageRepository, IRepository<LocaleStringResource> localeStringResourceRepository, IRepository<ActivityLogType> activityLogTypeRepository, IRepository<ActivityLog> activityLogRepository, IRepository<Log> logRepository, IRepository<Customer> customerRepository, IRepository<CustomerPassword> customerPasswordRepository, IRepository<ScheduleTask> scheduleTaskRepository, IRepository<CustomerRole> customerRoleRepository, IRepository<PermissionRecord> permissionRecordRepository, IRepository<EmailAccount> emailAccountRepository)
        {
            _blogCategoryRepository = blogCategoryRepository;
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
            _emailAccountRepository = emailAccountRepository;
        }

        #endregion

        #region Methods

        public virtual void InstallData(string defaultUserEmail, string defaultUserPassword, bool installSampleData = true)
        {
            InstallLanguages();
            InstallCustomersAndUsers(defaultUserEmail, defaultUserPassword);
            InstallEmailAccounts();
            InstallSettings(installSampleData);
            InstallLocaleResources();
            InstallActivityLogTypes();
            InstallScheduleTasks();
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
                FlagImageFileName = "us.png",
                Published = true,
                DisplayOrder = 3
            };
            _languageRepository.Insert(englishLanguage);

            var japanLanguage = new Language
            {
                Name = "日本語",
                LanguageCulture = "jp",
                UniqueSeoCode = "jp",
                FlagImageFileName = "jp.png",
                Published = true,
                DisplayOrder = 2
            };
            _languageRepository.Insert(japanLanguage);


            var chinaLanguage = new Language
            {
                Name = "中文简体",
                LanguageCulture = "zh-CN",
                UniqueSeoCode = "cn",
                FlagImageFileName = "cn.png",
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

            //后台任务（内置）用户
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
                DisplayJavaScriptDisabledWarning = true,
                DefaultTheme = null,
                Log404Errors = false,
                RenderXuaCompatible = true,
                XuaCompatibleValue = "IE=edge,chrome=1",
                UseStoredProcedureForLoadingCategories = false,
                UseStoredProceduresIfSupported = false
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
                AutomaticallyDetectLanguage = true,
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
                CustomerAuthenticationType = AuthenticationType.UsernameOrEmail,
                HashedPasswordFormat = "SHA1",
                FailedPasswordAllowedAttempts = 5,
                FailedPasswordLockoutMinutes = 10,
                UnduplicatedPasswordsNumber = 0,
                DefaultPasswordFormat = PasswordFormat.Hashed,
                OnlineCustomerMinutes = 60 * 24 * 7,
                StoreLastVisitedPage = true,
                CompanyEnabled = true,
                DateOfBirthEnabled = true,
                GenderEnabled = true,
                PhoneEnabled = true,
                CompanyRequired = false,
                PhoneRequired = false
            });

            settingService.SaveSetting(new DateTimeSettings
            {
                DefaultTimeZoneId = "",
                AllowCustomersToSetTimeZone = false
            });

            settingService.SaveSetting(new BlogSettings
            {
                AllowGuestsToCreateComments = false,
                MaxNumberOfTags = 15,
                BlogCommentsMustBeApproved = false,
                RichEditorAllowJavaScript = true
            });

            settingService.SaveSetting(new LogSettings
            {
                IgnoreLogWordlist = new List<string>()
            });

            settingService.SaveSetting(new MediaSettings
            {
                MaximumImageSize = 1980,
                DefaultImageQuality = 80,
                MultipleThumbDirectories = true
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

        protected virtual void InstallEmailAccounts()
        {
            var emailAccounts = new List<EmailAccount>
            {
                new EmailAccount
                {
                    Email = "test@mail.com",
                    DisplayName = "Blog name",
                    Host = "smtp.mail.com",
                    Port = 25,
                    Username = "123",
                    Password = "123",
                    EnableSsl = false,
                    UseDefaultCredentials = false
                },
            };
            _emailAccountRepository.Insert(emailAccounts);
        }

        protected virtual void InstallScheduleTasks()
        {
            var tasks = new List<ScheduleTask>
            {
                new ScheduleTask
                {
                    Name = "Clear Cache",
                    Seconds = 600,
                    Type = "RANSUROTTO.BLOG.Services.Caching.ClearCacheTask, RANSUROTTO.BLOG.Services",
                    Enabled = false,
                    StopOnError = false,
                },
                new ScheduleTask
                {
                    Name = "Delete guests",
                    Seconds = 600,
                    Type = "RANSUROTTO.BLOG.Services.Customers.DeleteGuestsTask, RANSUROTTO.BLOG.Services",
                    Enabled = false,
                    StopOnError = false,
                },
                new ScheduleTask
                {
                    Name = "Clear log",
                    Seconds = 3600,
                    Type = "RANSUROTTO.BLOG.Services.Logging.ClearLogTask, RANSUROTTO.BLOG.Services",
                    Enabled = false,
                    StopOnError = false,
                },
            };
            _scheduleTaskRepository.Insert(tasks);
        }

        #endregion

    }
}
