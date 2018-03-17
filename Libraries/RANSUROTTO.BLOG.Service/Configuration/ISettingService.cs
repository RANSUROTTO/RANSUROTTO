using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Domain.Configuration;

namespace RANSUROTTO.BLOG.Service.Configuration
{

    /// <summary>
    /// �趨ҵ���ӿ�
    /// </summary>
    public interface ISettingService
    {

        /// <summary>
        /// ͨ����ʶ����ȡ�趨��
        /// </summary>
        /// <param name="settingId">�趨���ʶ��</param>
        /// <returns>�趨��</returns>
        Setting GetSettingById(long settingId);

        /// <summary>
        /// ɾ���趨��
        /// </summary>
        /// <param name="setting">�趨��</param>
        void DeleteSetting(Setting setting);

        /// <summary>
        /// ɾ������趨��
        /// </summary>
        /// <param name="settings">����趨��</param>
        void DeleteSettings(IList<Setting> settings);

        /// <summary>
        /// ͨ������ȡ�趨��
        /// </summary>
        /// <param name="key">��</param>
        /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all stores) value should be loaded if a value specific for a certain is not found</param>
        /// <returns>�趨��</returns>
        Setting GetSetting(string key, bool loadSharedValueIfNotFound = false);

        /// <summary>
        /// ͨ������ȡָ���趨��ֵ
        /// </summary>
        /// <typeparam name="T">ֵ������</typeparam>
        /// <param name="key">��</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <param name="loadSharedValueIfNotFound">A value indicating whether a shared (for all stores) value should be loaded if a value specific for a certain is not found</param>
        /// <returns>�趨��ֵ</returns>
        T GetSettingByKey<T>(string key, T defaultValue = default(T),
             bool loadSharedValueIfNotFound = false);

        /// <summary>
        /// �����趨��ֵ
        /// </summary>
        /// <typeparam name="T">ֵ������</typeparam>
        /// <param name="key">��</param>
        /// <param name="value">ֵ</param>
        /// <param name="clearCache">�Ƿ���Ҫ�������</param>
        void SetSetting<T>(string key, T value, bool clearCache = true);

        /// <summary>
        /// ��ȡ�����趨��
        /// </summary>
        /// <returns>�趨���б�</returns>
        IList<Setting> GetAllSettings();

        /// <summary>
        /// ����Ƿ����ĳ���趨��
        /// </summary>
        /// <typeparam name="T">�趨����</typeparam>
        /// <typeparam name="TPropType">�趨��</typeparam>
        /// <param name="settings">�趨ʵ��</param>
        /// <param name="keySelector">�趨��ѡ��</param>
        /// <returns>������ڷ���true,���򷵻�false</returns>
        bool SettingExists<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new();

        /// <summary>
        /// ��ȡ�趨
        /// </summary>
        /// <typeparam name="T">�趨����</typeparam>
        T LoadSetting<T>() where T : ISettings, new();

        /// <summary>
        /// �����趨
        /// </summary>
        /// <typeparam name="T">�趨����</typeparam>
        /// <param name="settings">�趨ʵ��</param>
        void SaveSetting<T>(T settings) where T : ISettings, new();

        /// <summary>
        /// �����趨 (�����ض�����)
        /// </summary>
        /// <typeparam name="T">�趨����</typeparam>
        /// <typeparam name="TPropType">�趨��</typeparam>
        /// <param name="settings">�趨ʵ��</param>
        /// <param name="keySelector">�趨��ѡ��</param>
        /// <param name="clearCache">�Ƿ���Ҫ�������</param>
        void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            bool clearCache = true) where T : ISettings, new();

        /// <summary>
        /// Save settings object (per store). If the setting is not overridden per storem then it'll be delete
        /// </summary>
        /// <typeparam name="T">Entity type</typeparam>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="settings">Settings</param>
        /// <param name="keySelector">Key selector</param>
        /// <param name="overrideForStore">A value indicating whether to setting is overridden in some store</param>
        /// <param name="clearCache">A value indicating whether to clear cache after setting update</param>
        void SaveSettingOverridablePerStore<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            bool overrideForStore, bool clearCache = true) where T : ISettings, new();

        /// <summary>
        /// ɾ���趨
        /// </summary>
        /// <typeparam name="T">�趨����</typeparam>
        void DeleteSetting<T>() where T : ISettings, new();

        /// <summary>
        /// ɾ���趨 (ɾ���ض�����)
        /// </summary>
        /// <typeparam name="T">�趨����</typeparam>
        /// <typeparam name="TPropType">�趨��</typeparam>
        /// <param name="settings">�趨ʵ��</param>
        /// <param name="keySelector">�趨��ѡ��</param>
        void DeleteSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector) where T : ISettings, new();

        /// <summary>
        /// �������
        /// </summary>
        void ClearCache();

    }

}
