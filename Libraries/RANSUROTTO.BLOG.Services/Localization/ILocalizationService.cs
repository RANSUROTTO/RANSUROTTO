using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Services.Localization
{

    /// <summary>
    /// ���򻯹���ӿ�
    /// </summary>
    public interface ILocalizationService
    {

        /// <summary>
        /// ɾ�����������ַ�����Դ
        /// </summary>
        /// <param name="localeStringResource">���������ַ�����Դ</param>
        void DeleteLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// ��ȡ�����������������ַ�����Դ
        /// </summary>
        /// <param name="localeStringResourceId">���������ַ�����Դ��ʶ��</param>
        /// <returns>���������ַ�����Դ</returns>
        LocaleStringResource GetLocaleStringResourceById(int localeStringResourceId);

        /// <summary>
        /// ͨ����Դ�����뵱ǰ�������Ի�ȡ���������ַ�����Դ
        /// </summary>
        /// <param name="resourceName">��Դ����</param>
        /// <returns>���������ַ�����Դ</returns>
        LocaleStringResource GetLocaleStringResourceByName(string resourceName);

        /// <summary>
        /// ��ȡ���������ַ�����Դ
        /// </summary>
        /// <param name="resourceName">��Դ����</param>
        /// <param name="languageId">���Ա�ʶ��</param>
        /// <param name="logIfNotFound">����Ҳ������������ַ�����Դ,ָʾ�Ƿ�Ҫ��¼����</param>
        /// <returns>���������ַ�����Դ</returns>
        LocaleStringResource GetLocaleStringResourceByName(string resourceName, int languageId,
            bool logIfNotFound = true);

        /// <summary>
        /// ͨ�����Ա�ʶ����ȡ�������������ַ�����Դ
        /// </summary>
        /// <param name="languageId">���Ա�ʶ��</param>
        /// <returns>���������ַ�����Դ�б�</returns>
        IList<LocaleStringResource> GetAllResources(int languageId);

        /// <summary>
        /// �������������ַ�����Դ
        /// </summary>
        /// <param name="localeStringResource">���������ַ�����Դ</param>
        void InsertLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// �������������ַ�����Դ
        /// </summary>
        /// <param name="localeStringResource">���������ַ�����Դ</param>
        void UpdateLocaleStringResource(LocaleStringResource localeStringResource);

        /// <summary>
        /// ͨ�����Ա�ʶ����ȡ�������������ַ�����Դ
        /// </summary>
        /// <param name="languageId">���Ա�ʶ��</param>
        /// <returns>���������������ַ�����Դ�ֵ�</returns>
        Dictionary<string, KeyValuePair<int, string>> GetAllResourceValues(int languageId);

        /// <summary>
        /// ͨ����Դ��ֵ��ȡ���������ַ�����Դֵ
        /// </summary>
        /// <param name="resourceKey">��Դ��ֵ</param>
        /// <returns>���������ַ�����Դֵ</returns>
        string GetResource(string resourceKey);

        /// <summary>
        /// ͨ����Դ��ֵ��ȡ���������ַ�����Դֵ
        /// </summary>
        /// <param name="resourceKey">��Դ��ֵ</param>
        /// <param name="languageId">���Ա�ʶ��</param>
        /// <param name="logIfNotFound">����Ҳ������������ַ�����Դֵ,ָʾ�Ƿ�Ҫ��¼����</param>
        /// <param name="defaultValue">Ĭ��ֵ</param>
        /// <param name="returnEmptyIfNotFound">ָʾδ�ҵ���Դʱ�Ƿ񷵻ؿ��ַ���,����Ĭ��ֵ����Ϊ���ַ���</param>
        /// <returns>���������ַ�����Դֵ</returns>
        string GetResource(string resourceKey, int languageId,
            bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false);

        /// <summary>
        /// ��������Դ������xml
        /// </summary>
        /// <param name="language">����</param>
        /// <returns>XML��ʽ�Ľ��</returns>
        string ExportResourcesToXml(Language language);

        /// <summary>
        /// ��XML�ļ�����������Դ
        /// </summary>
        /// <param name="language">����</param>
        /// <param name="xml">XML��ʽ�Ľ��</param>
        /// <param name="updateExistingResources">ָʾ�Ƿ����������Դ��ֵ</param>
        void ImportResourcesFromXml(Language language, string xml, bool updateExistingResources = true);

    }

}
