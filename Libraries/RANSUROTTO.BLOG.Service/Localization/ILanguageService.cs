using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Service.Localization
{

    /// <summary>
    /// 语言业务层接口
    /// </summary>
    public interface ILanguageService
    {

        /// <summary>
        /// 获取所有语言
        /// </summary>
        /// <param name="showHidden">是否显示已隐藏的语言</param>
        /// <returns>语言列表</returns>
        IList<Language> GetAllLanguages(bool showHidden = false);

        /// <summary>
        /// 通过标识符获取语言
        /// </summary>
        /// <param name="languageId">语言标识符</param>
        /// <returns>语言</returns>
        Language GetLanguageById(long languageId);

        /// <summary>
        /// 添加语言
        /// </summary>
        /// <param name="language">语言</param>
        void InsertLanguage(Language language);

        /// <summary>
        /// 更新语言
        /// </summary>
        /// <param name="language">语言</param>
        void UpdateLanguage(Language language);

        /// <summary>
        /// 删除语言
        /// </summary>
        /// <param name="language">语言</param>
        void DeleteLanguage(Language language);

    }
}
