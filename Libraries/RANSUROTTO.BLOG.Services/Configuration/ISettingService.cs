using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Domain.Configuration;

namespace RANSUROTTO.BLOG.Services.Configuration
{

    /// <summary>
    /// 设定业务层接口
    /// </summary>
    public interface ISettingService
    {

        /// <summary>
        /// 通过标识符获取设定项
        /// </summary>
        /// <param name="settingId">设定项标识符</param>
        /// <returns>设定项</returns>
        Setting GetSettingById(long settingId);

        /// <summary>
        /// 删除设定项
        /// </summary>
        /// <param name="setting">设定项</param>
        void DeleteSetting(Setting setting);

        /// <summary>
        /// 删除多个设定项
        /// </summary>
        /// <param name="settings">多个设定项</param>
        void DeleteSettings(IList<Setting> settings);

        /// <summary>
        /// 通过键获取设定项
        /// </summary>
        /// <param name="key">键</param>
        /// <returns>设定项</returns>
        Setting GetSetting(string key);

        /// <summary>
        /// 通过键获取指定设定项值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>设定项值</returns>
        T GetSettingByKey<T>(string key, T defaultValue = default(T));

        /// <summary>
        /// 设置设定项值
        /// </summary>
        /// <typeparam name="T">值的类型</typeparam>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="clearCache">是否需要清除缓存</param>
        void SetSetting<T>(string key, T value, bool clearCache = true);

        /// <summary>
        /// 获取所有设定项
        /// </summary>
        /// <returns>设定项列表</returns>
        IList<Setting> GetAllSettings();

        /// <summary>
        /// 检查是否存在某个设定项
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        /// <typeparam name="TPropType">设定项类型</typeparam>
        /// <param name="settings">设定实例</param>
        /// <param name="keySelector">设定项选择</param>
        /// <returns>如果存在返回true,否则返回false</returns>
        bool SettingExists<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new();

        /// <summary>
        /// 获取设定
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        T LoadSetting<T>() where T : ISettings, new();

        /// <summary>
        /// 保存设定
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        /// <param name="settings">设定实例</param>
        void SaveSetting<T>(T settings) where T : ISettings, new();

        /// <summary>
        /// 保存设定 (保存特定的项)
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        /// <typeparam name="TPropType">设定项类型</typeparam>
        /// <param name="settings">设定实例</param>
        /// <param name="keySelector">设定项选择</param>
        /// <param name="clearCache">是否需要清除缓存</param>
        void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            bool clearCache = true) where T : ISettings, new();

        /// <summary>
        /// 删除设定
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        void DeleteSetting<T>() where T : ISettings, new();

        /// <summary>
        /// 删除设定 (删除特定的项)
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        /// <typeparam name="TPropType">设定项</typeparam>
        /// <param name="settings">设定实例</param>
        /// <param name="keySelector">设定项选择</param>
        void DeleteSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector) where T : ISettings, new();

        /// <summary>
        /// 清除缓存
        /// </summary>
        void ClearCache();

    }

}
