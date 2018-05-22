using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Domain.Localization.Setting;
using RANSUROTTO.BLOG.Core.Helper;

namespace RANSUROTTO.BLOG.Services.Localization
{
    public class LocalizedEntityService : ILocalizedEntityService
    {

        #region Constants

        /// <summary>
        /// 区域化属性缓存
        /// </summary>
        /// <remarks>
        /// {0} : 语言标识符
        /// {1} : 对应实体标识符
        /// {2} : 区域化属性分类
        /// {3} : 区域化属性键
        /// </remarks>
        private const string LOCALIZEDPROPERTY_KEY = "Ransurotto.localizedproperty.value-{0}-{1}-{2}-{3}";

        /// <summary>
        /// 区域化属性缓存
        /// </summary>
        private const string LOCALIZEDPROPERTY_ALL_KEY = "Ransurotto.localizedproperty.all";

        /// <summary>
        /// 清除缓存键匹配模式
        /// </summary>
        private const string LOCALIZEDPROPERTY_PATTERN_KEY = "Ransurotto.localizedproperty.";

        #endregion

        #region Fields

        private readonly IRepository<LocalizedProperty> _localizedPropertyRepository;
        private readonly ICacheManager _cacheManager;
        private readonly LocalizationSettings _localizationSettings;

        #endregion

        #region Constructor

        public LocalizedEntityService(IRepository<LocalizedProperty> localizedPropertyRepository, ICacheManager cacheManager, LocalizationSettings localizationSettings)
        {
            _localizedPropertyRepository = localizedPropertyRepository;
            _cacheManager = cacheManager;
            _localizationSettings = localizationSettings;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 通过标识符获取实体区域化属性
        /// </summary>
        /// <param name="localizedPropertyId">实体区域化属性标识符</param>
        /// <returns>实体区域化属性</returns>
        public virtual LocalizedProperty GetLocalizedPropertyById(int localizedPropertyId)
        {
            if (localizedPropertyId == 0)
                return null;

            return _localizedPropertyRepository.GetById(localizedPropertyId);
        }

        /// <summary>
        /// 获取实体区域化属性值
        /// </summary>
        /// <param name="languageId">语言标识符</param>
        /// <param name="entityId">对应实体标识符</param>
        /// <param name="localeKeyGroup">区域化属性类型</param>
        /// <param name="localeKey">属性键</param>
        /// <returns>实体区域化属性</returns>
        public virtual string GetLocalizedValue(int languageId, int entityId, string localeKeyGroup, string localeKey)
        {
            if (_localizationSettings.LoadAllLocalizedPropertiesOnStartup)
            {
                //从已经加载好所有区域化属性的缓存中查找对应区域化属性值
                string key = string.Format(LOCALIZEDPROPERTY_KEY, languageId, entityId, localeKeyGroup, localeKey);
                return _cacheManager.Get(key, () =>
                {
                    var source = GetAllLocalizedPropertiesCached();
                    var query = from lp in source
                                where lp.LanguageId == languageId &&
                                      lp.EntityId == entityId &&
                                      lp.LocaleKeyGroup == localeKeyGroup &&
                                      lp.LocaleKey == localeKey
                                select lp.LocaleValue;
                    var localeValue = query.FirstOrDefault();
                    //空值不能进行缓存,设置为空字符串
                    if (localeValue == null)
                        localeValue = "";
                    return localeValue;
                });
            }
            else
            {
                //从数据库中查找对应区域化属性值
                string key = string.Format(LOCALIZEDPROPERTY_KEY, languageId, entityId, localeKeyGroup, localeKey);
                return _cacheManager.Get(key, () =>
                {
                    var source = _localizedPropertyRepository.Table;
                    var query = from lp in source
                                where lp.LanguageId == languageId &&
                                      lp.EntityId == entityId &&
                                      lp.LocaleKeyGroup == localeKeyGroup &&
                                      lp.LocaleKey == localeKey
                                select lp.LocaleValue;
                    var localeValue = query.FirstOrDefault();
                    //空值不能进行缓存,设置为空字符串
                    if (localeValue == null)
                        localeValue = "";
                    return localeValue;
                });
            }
        }

        /// <summary>
        /// 添加实体区域化属性
        /// </summary>
        /// <param name="localizedProperty">实体区域化属性</param>
        public virtual void InsertLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException(nameof(localizedProperty));

            _localizedPropertyRepository.Insert(localizedProperty);

            _cacheManager.RemoveByPattern(LOCALIZEDPROPERTY_PATTERN_KEY);
        }

        /// <summary>
        /// 更新实体区域化属性
        /// </summary>
        /// <param name="localizedProperty">实体区域化属性</param>
        public virtual void UpdateLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException(nameof(localizedProperty));

            _localizedPropertyRepository.Update(localizedProperty);

            _cacheManager.RemoveByPattern(LOCALIZEDPROPERTY_PATTERN_KEY);
        }

        /// <summary>
        /// 保存实体区域化属性值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">泛型实例</param>
        /// <param name="keySelector">属性类型选择</param>
        /// <param name="localeValue">属性值</param>
        /// <param name="languageId">语言标识符</param>
        public virtual void SaveLocalizedValue<T>(T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            int languageId) where T : BaseEntity, ILocalizedEntity
        {
            SaveLocalizedValue<T, string>(entity, keySelector, localeValue, languageId);
        }

        /// <summary>
        /// 保存实体区域化属性值
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <typeparam name="TPropType">属性值泛型</typeparam>
        /// <param name="entity">泛型实例</param>
        /// <param name="keySelector">属性类型选择</param>
        /// <param name="localeValue">属性值</param>
        /// <param name="languageId">语言标识符</param>
        public virtual void SaveLocalizedValue<T, TPropType>(T entity,
            Expression<Func<T, TPropType>> keySelector,
            TPropType localeValue,
            int languageId) where T : BaseEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (languageId == 0)
                throw new ArgumentOutOfRangeException(nameof(languageId), "语言ID不应为0");

            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "表达式 '{0}' 不是一个属性选择器",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                       "表达式 '{0}' 不是一个属性",
                       keySelector));
            }

            string localeKeyGroup = typeof(T).Name;
            string localeKey = propInfo.Name;

            var props = GetLocalizedProperties(entity.Id, localeKeyGroup);
            var prop = props.FirstOrDefault(lp => lp.LanguageId == languageId &&
                lp.LocaleKey.Equals(localeKey, StringComparison.InvariantCultureIgnoreCase)); //should be culture invariant

            var localeValueStr = CommonHelper.To<string>(localeValue);

            if (prop != null)
            {
                if (string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //delete
                    DeleteLocalizedProperty(prop);
                }
                else
                {
                    //update
                    prop.LocaleValue = localeValueStr;
                    UpdateLocalizedProperty(prop);
                }
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(localeValueStr))
                {
                    //insert
                    prop = new LocalizedProperty
                    {
                        EntityId = entity.Id,
                        LanguageId = languageId,
                        LocaleKey = localeKey,
                        LocaleKeyGroup = localeKeyGroup,
                        LocaleValue = localeValueStr
                    };
                    InsertLocalizedProperty(prop);
                }
            }
        }

        /// <summary>
        /// 删除实体区域化属性
        /// </summary>
        /// <param name="localizedProperty">实体区域化属性</param>
        public virtual void DeleteLocalizedProperty(LocalizedProperty localizedProperty)
        {
            if (localizedProperty == null)
                throw new ArgumentNullException(nameof(localizedProperty));

            _localizedPropertyRepository.Delete(localizedProperty);

            _cacheManager.RemoveByPattern(LOCALIZEDPROPERTY_PATTERN_KEY);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 获取指定类型和对应实体的区域化属性列表
        /// </summary>
        /// <param name="entityId">对应实体标识符</param>
        /// <param name="localeKeyGroup">区域属性类别</param>
        /// <returns></returns>
        protected virtual IList<LocalizedProperty> GetLocalizedProperties(int entityId, string localeKeyGroup)
        {
            if (entityId == 0 || string.IsNullOrEmpty(localeKeyGroup))
                return new List<LocalizedProperty>();

            var query = from lp in _localizedPropertyRepository.Table
                        orderby lp.Id
                        where lp.EntityId == entityId &&
                              lp.LocaleKeyGroup == localeKeyGroup
                        select lp;
            var props = query.ToList();
            return props;
        }

        /// <summary>
        /// 获取所有区域化属性（并进行缓存）
        /// </summary>
        /// <returns>区域化属性列表</returns>
        protected virtual IList<LocalizedPropertyForCaching> GetAllLocalizedPropertiesCached()
        {
            string key = string.Format(LOCALIZEDPROPERTY_ALL_KEY);
            return _cacheManager.Get(key, () =>
            {
                var query = from lp in _localizedPropertyRepository.Table
                            select lp;
                var localizedProperties = query.ToList();
                var list = new List<LocalizedPropertyForCaching>();
                foreach (var lp in localizedProperties)
                {
                    var localizedPropertyForCaching = new LocalizedPropertyForCaching
                    {
                        Id = lp.Id,
                        EntityId = lp.EntityId,
                        LanguageId = lp.LanguageId,
                        LocaleKeyGroup = lp.LocaleKeyGroup,
                        LocaleKey = lp.LocaleKey,
                        LocaleValue = lp.LocaleValue
                    };
                    list.Add(localizedPropertyForCaching);
                }
                return list;
            });
        }

        #endregion

        #region Nested classes

        [Serializable]
        public class LocalizedPropertyForCaching
        {
            public int Id { get; set; }
            public int EntityId { get; set; }
            public int LanguageId { get; set; }
            public string LocaleKeyGroup { get; set; }
            public string LocaleKey { get; set; }
            public string LocaleValue { get; set; }
        }

        #endregion

    }
}
