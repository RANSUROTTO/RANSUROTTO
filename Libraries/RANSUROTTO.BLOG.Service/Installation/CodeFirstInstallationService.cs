using System;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Blog;
using RANSUROTTO.BLOG.Core.Domain.Common;
using RANSUROTTO.BLOG.Core.Domain.Configuration;
using RANSUROTTO.BLOG.Core.Domain.Customers;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Domain.Logging;
using RANSUROTTO.BLOG.Core.Domain.Tasks;

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

        #endregion

        #region Constructor

        public CodeFirstInstallationService(IRepository<Category> categoryRepository, IRepository<BlogPost> blogPostRepository, IRepository<BlogComment> blogCommentRepository, IRepository<GenericAttribute> genericAttributeRepository, IRepository<Setting> settingRepository, IRepository<Language> languageRepository, IRepository<LocaleStringResource> localeStringResourceRepository, IRepository<ActivityLogType> activityLogTypeRepository, IRepository<ActivityLog> activityLogRepository, IRepository<Log> logRepository, IRepository<Customer> customerRepository, IRepository<CustomerPassword> customerPasswordRepository, IRepository<ScheduleTask> scheduleTaskRepository)
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

        }

        protected virtual void InstallSettings(bool installSampleData)
        {

        }

        protected virtual void InstallActivityLogTypes()
        {

        }

        protected virtual void InstallScheduleTasks()
        {

        }

        #endregion

    }
}
