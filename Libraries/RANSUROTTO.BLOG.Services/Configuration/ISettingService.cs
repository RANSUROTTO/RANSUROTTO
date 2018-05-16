using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Domain.Configuration;

namespace RANSUROTTO.BLOG.Services.Configuration
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
        /// <returns>�趨��</returns>
        Setting GetSetting(string key);

        /// <summary>
        /// ͨ������ȡָ���趨��ֵ
        /// </summary>
        /// <typeparam name="T">ֵ������</typeparam>
        /// <param name="key">��</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <returns>�趨��ֵ</returns>
        T GetSettingByKey<T>(string key, T defaultValue = default(T));

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
        /// <typeparam name="TPropType">�趨������</typeparam>
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
        /// <typeparam name="TPropType">�趨������</typeparam>
        /// <param name="settings">�趨ʵ��</param>
        /// <param name="keySelector">�趨��ѡ��</param>
        /// <param name="clearCache">�Ƿ���Ҫ�������</param>
        void SaveSetting<T, TPropType>(T settings,
            Expression<Func<T, TPropType>> keySelector,
            bool clearCache = true) where T : ISettings, new();

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
