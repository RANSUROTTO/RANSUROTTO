using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Configuration;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Services.Events;

namespace RANSUROTTO.BLOG.Services.Configuration
{
    public class SettingService : ISettingService
    {

        #region Constants

        /// <summary>
        /// 所有设定缓存
        /// </summary>
        private const string SETTINGS_ALL_KEY = "Ransurotto.setting.all";

        /// <summary>
        /// 清除缓存的键匹配模式
        /// </summary>
        private const string SETTINGS_PATTERN_KEY = "Ransurotto.setting.";

        #endregion

        #region Fields

        private readonly ICacheManager _cacheManager;
        private readonly IRepository<Setting> _settingRepository;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Constructor

        public SettingService(ICacheManager cacheManager, IRepository<Setting> settingRepository, IEventPublisher eventPublisher)
        {
            _cacheManager = cacheManager;
            _settingRepository = settingRepository;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        /// <summary>
        /// 添加设定项
        /// </summary>
        /// <param name="setting">设定项实例</param>
        /// <param name="clearCache">是否清空缓存</param>
        public virtual void InsertSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            _settingRepository.Insert(setting);

            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

            _eventPublisher.EntityInserted(setting);
        }

        /// <summary>
        /// 更新设定项
        /// </summary>
        /// <param name="setting">设定项</param>
        /// <param name="clearCache">是否清空缓存</param>
        public virtual void UpdateSetting(Setting setting, bool clearCache = true)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            _settingRepository.Update(setting);

            if (clearCache)
                _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

            _eventPublisher.EntityUpdated(setting);
        }

        /// <summary>
        /// 删除设定项
        /// </summary>
        /// <param name="setting">设定项</param>
        public virtual void DeleteSetting(Setting setting)
        {
            if (setting == null)
                throw new ArgumentNullException("setting");

            _settingRepository.Delete(setting);

            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

            _eventPublisher.EntityDeleted(setting);
        }

        /// <summary>
        /// 删除多个设定项
        /// </summary>
        /// <param name="settings">多个设定项</param>
        public virtual void DeleteSettings(IList<Setting> settings)
        {
            if (settings == null)
                throw new ArgumentNullException("settings");

            _settingRepository.Delete(settings);

            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);

            foreach (var setting in settings)
            {
                _eventPublisher.EntityDeleted(setting);
            }
        }

        /// <summary>
        /// 通过标识符获取设定项
        /// </summary>
        /// <param name="settingId">设定项标识符</param>
        /// <returns>设定项</returns>
        public virtual Setting GetSettingById(int settingId)
        {
            if (settingId == 0)
                return null;

            return _settingRepository.GetById(settingId);
        }

        /// <summary>
        /// 通过键获取设定项
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>设定项</returns>
        public virtual Setting GetSetting(string key)
        {
            if (String.IsNullOrEmpty(key))
                return null;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var settingsByKey = settings[key];
                var setting = settingsByKey.FirstOrDefault();

                if (setting != null)
                    return GetSettingById(setting.Id);
            }

            return null;
        }

        /// <summary>
        /// 通过键获取设定项
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>设定项值</returns>
        public virtual T GetSettingByKey<T>(string key, T defaultValue = default(T))
        {
            if (String.IsNullOrEmpty(key))
                return defaultValue;

            var settings = GetAllSettingsCached();
            key = key.Trim().ToLowerInvariant();
            if (settings.ContainsKey(key))
            {
                var settingsByKey = settings[key];
                var setting = settingsByKey.FirstOrDefault();

                if (setting != null)
                    return CommonHelper.To<T>(setting.Value);
            }

            return defaultValue;
        }

        /// <summary>
        /// 设置设定项值
        /// </summary>
        /// <typeparam name="T">值类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="clearCache">是否清空缓存</param>
        public virtual void SetSetting<T>(string key, T value, bool clearCache = true)
        {
            if (key == null)
                throw new ArgumentNullException("key");

            key = key.Trim().ToLowerInvariant();
            string valueStr = TypeDescriptor.GetConverter(typeof(T)).ConvertToInvariantString(value);

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault() : null;

            if (settingForCaching != null)
            {
                var setting = GetSettingById(settingForCaching.Id);
                setting.Value = valueStr;
                UpdateSetting(setting, clearCache);
            }
            else
            {
                var setting = new Setting
                {
                    Name = key,
                    Value = valueStr
                };
                InsertSetting(setting, clearCache);
            }
        }

        /// <summary>
        /// 获取全部设定项
        /// </summary>
        /// <returns>设定列表</returns>
        public virtual IList<Setting> GetAllSettings()
        {
            var query = from s in _settingRepository.Table
                        orderby s.Name
                        select s;
            var settings = query.ToList();
            return settings;
        }

        /// <summary>
        /// 检查某个设定项是否存在
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        /// <typeparam name="TPropType">设定项类型</typeparam>
        /// <param name="settings">设定实例</param>
        /// <param name="keySelector">设定项选择</param>
        /// <returns>如果存在返回true,否则返回false</returns>
        public virtual bool SettingExists<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new()
        {
            string key = settings.GetSettingKey(keySelector);

            var setting = GetSettingByKey<string>(key);
            return setting != null;
        }

        /// <summary>
        /// 获取设定
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        /// <returns>设定实例</returns>
        public virtual T LoadSetting<T>() where T : ISettings, new()
        {
            var settings = Activator.CreateInstance<T>();

            foreach (var prop in typeof(T).GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                var key = typeof(T).Name + "." + prop.Name;

                var setting = GetSettingByKey<string>(key);
                if (setting == null)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).IsValid(setting))
                    continue;

                object value = TypeDescriptor.GetConverter(prop.PropertyType).ConvertFromInvariantString(setting);

                prop.SetValue(settings, value, null);
            }

            return settings;
        }

        /// <summary>
        /// 保存设定
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        /// <param name="settings">设定实例</param>
        public virtual void SaveSetting<T>(T settings) where T : ISettings, new()
        {
            foreach (var prop in typeof(T).GetProperties())
            {
                if (!prop.CanRead || !prop.CanWrite)
                    continue;

                if (!TypeDescriptor.GetConverter(prop.PropertyType).CanConvertFrom(typeof(string)))
                    continue;

                string key = typeof(T).Name + "." + prop.Name;

                dynamic value = prop.GetValue(settings, null);
                if (value != null)
                    SetSetting(key, value);
                else
                    SetSetting(key, "");
            }

            ClearCache();
        }

        /// <summary>
        /// 保存设定（保存特定的项）
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        /// <typeparam name="TPropType">设定项类型</typeparam>
        /// <param name="settings">设定实例</param>
        /// <param name="keySelector">设定项选择</param>
        /// <param name="clearCache">是否需要清除缓存</param>
        public virtual void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector, bool clearCache = true) where T : ISettings, new()
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    keySelector));
            }

            string key = settings.GetSettingKey(keySelector);

            dynamic value = propInfo.GetValue(settings, null);
            if (value != null)
                SetSetting(key, value, clearCache);
            else
                SetSetting(key, "", clearCache);
        }

        /// <summary>
        /// 删除设定（删除设定内的所有设定项）
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        public virtual void DeleteSetting<T>() where T : ISettings, new()
        {
            var settingsToDelete = new List<Setting>();
            var allSettings = GetAllSettings();
            foreach (var prop in typeof(T).GetProperties())
            {
                string key = typeof(T).Name + "." + prop.Name;
                settingsToDelete.AddRange(allSettings.Where(x => x.Name.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
            }

            DeleteSettings(settingsToDelete);
        }

        /// <summary>
        /// 删除设定（删除特定的项）
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        /// <typeparam name="TPropType">设定项类型</typeparam>
        /// <param name="settings">设定实例</param>
        /// <param name="keySelector">设定项选择</param>
        public virtual void DeleteSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector) where T : ISettings, new()
        {
            string key = settings.GetSettingKey(keySelector);
            key = key.Trim().ToLowerInvariant();

            var allSettings = GetAllSettingsCached();
            var settingForCaching = allSettings.ContainsKey(key) ?
                allSettings[key].FirstOrDefault() : null;
            if (settingForCaching != null)
            {
                var setting = GetSettingById(settingForCaching.Id);
                DeleteSetting(setting);
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public virtual void ClearCache()
        {
            _cacheManager.RemoveByPattern(SETTINGS_PATTERN_KEY);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 获取所有已缓存的设定
        /// </summary>
        /// <returns>设定列表</returns>
        protected virtual IDictionary<string, IList<SettingForCaching>> GetAllSettingsCached()
        {
            string key = string.Format(SETTINGS_ALL_KEY);
            return _cacheManager.Get(key, () =>
            {
                //使用不跟踪状态查询来优化性能
                var query = from s in _settingRepository.TableNoTracking
                            orderby s.Name
                            select s;
                var settings = query.ToList();

                var dictionary = new Dictionary<string, IList<SettingForCaching>>();
                foreach (var s in settings)
                {
                    var resourceName = s.Name.ToLowerInvariant();
                    var settingForCaching = new SettingForCaching
                    {
                        Id = s.Id,
                        Name = s.Name,
                        Value = s.Value
                    };
                    if (!dictionary.ContainsKey(resourceName))
                    {
                        dictionary.Add(resourceName, new List<SettingForCaching>
                        {
                            settingForCaching
                        });
                    }
                    else
                    {
                        dictionary[resourceName].Add(settingForCaching);
                    }
                }
                return dictionary;
            });
        }

        #endregion

        #region Nested classes

        /// <summary>
        /// 设定缓存类
        /// </summary>
        [Serializable]
        public class SettingForCaching
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Value { get; set; }
        }

        #endregion

    }
}
