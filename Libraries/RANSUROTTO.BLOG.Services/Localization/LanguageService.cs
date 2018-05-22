using System;
using System.Collections.Generic;
using System.Linq;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Domain.Localization.Setting;
using RANSUROTTO.BLOG.Services.Configuration;
using RANSUROTTO.BLOG.Services.Events;

namespace RANSUROTTO.BLOG.Services.Localization
{
    public class LanguageService : ILanguageService
    {

        #region Constants

        /// <summary>
        /// 语言缓存
        /// </summary>
        /// <remarks>
        /// {0} : 语言ID
        /// </remarks>
        private const string LANGUAGES_BY_ID_KEY = "Ransurotto.language.id-{0}";

        /// <summary>
        /// 语言列表缓存
        /// </summary>
        /// <remarks>
        /// {0} : 指示是否显示隐藏的缓存
        /// </remarks>
        private const string LANGUAGES_ALL_KEY = "Ransurotto.language.all-{0}";

        /// <summary>
        /// 清除缓存的键匹配模式
        /// </summary>
        private const string LANGUAGES_PATTERN_KEY = "Ransurotto.language.";

        #endregion

        #region Fields

        private readonly IRepository<Language> _languageRepository;
        private readonly ICacheManager _cacheManager;
        private readonly ISettingService _settingService;
        private readonly IEventPublisher _eventPublisher;
        private readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Constructor

        public LanguageService(IRepository<Language> languageRepository, ICacheManager cacheManager, ISettingService settingService, IEventPublisher eventPublisher, LocalizationSettings localizationSettings)
        {
            _languageRepository = languageRepository;
            _cacheManager = cacheManager;
            _settingService = settingService;
            _eventPublisher = eventPublisher;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取所有语言
        /// </summary>
        /// <param name="showHidden">是否显示已隐藏的语言</param>
        /// <returns>语言列表</returns>
        public virtual IList<Language> GetAllLanguages(bool showHidden = false)
        {
            string key = string.Format(LANGUAGES_ALL_KEY, showHidden);
            var languages = _cacheManager.Get(key, () =>
            {
                var query = _languageRepository.Table;
                if (!showHidden)
                    query = query.Where(l => l.Published);
                query = query.OrderBy(l => l.DisplayOrder).ThenBy(l => l.Id);
                return query.ToList();
            });

            return languages;
        }

        /// <summary>
        /// 通过标识符获取语言
        /// </summary>
        /// <param name="languageId">语言标识符</param>
        /// <returns>语言</returns>
        public virtual Language GetLanguageById(int languageId)
        {
            if (languageId == 0)
                return null;

            string key = string.Format(LANGUAGES_BY_ID_KEY, languageId);
            return _cacheManager.Get(key, () => _languageRepository.GetById(languageId));
        }

        /// <summary>
        /// 添加语言
        /// </summary>
        /// <param name="language">语言</param>
        public virtual void InsertLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException("language");

            _languageRepository.Insert(language);

            _cacheManager.RemoveByPattern(LANGUAGES_PATTERN_KEY);

            _eventPublisher.EntityInserted(language);
        }

        /// <summary>
        /// 更新语言
        /// </summary>
        /// <param name="language">语言</param>
        public virtual void UpdateLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            _languageRepository.Update(language);

            _cacheManager.RemoveByPattern(LANGUAGES_PATTERN_KEY);

            _eventPublisher.EntityUpdated(language);
        }

        /// <summary>
        /// 删除语言
        /// </summary>
        /// <param name="language">语言</param>
        public virtual void DeleteLanguage(Language language)
        {
            if (language == null)
                throw new ArgumentNullException(nameof(language));

            //如果语言被删除，需要更新默认管理员区域语言Id
            if (_localizationSettings.DefaultAdminLanguageId == language.Id)
            {
                foreach (var activeLanguage in GetAllLanguages())
                {
                    if (activeLanguage.Id != language.Id)
                    {
                        _localizationSettings.DefaultAdminLanguageId = activeLanguage.Id;
                        _settingService.SaveSetting(_localizationSettings);
                        break;
                    }
                }
            }

            _languageRepository.Delete(language);

            _cacheManager.RemoveByPattern(LANGUAGES_PATTERN_KEY);

            _eventPublisher.EntityDeleted(language);
        }

        #endregion

    }
}
