using System;
using System.Linq.Expressions;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Localization;

namespace RANSUROTTO.BLOG.Services.Localization
{
    public interface ILocalizedEntityService
    {

        /// <summary>
        /// 通过标识符获取实体区域化属性
        /// </summary>
        /// <param name="localizedPropertyId">实体区域化属性标识符</param>
        /// <returns>实体区域化属性</returns>
        LocalizedProperty GetLocalizedPropertyById(int localizedPropertyId);

        /// <summary>
        /// 获取实体区域化属性值
        /// </summary>
        /// <param name="languageId">语言标识符</param>
        /// <param name="entityId">对应实体标识符</param>
        /// <param name="localeKeyGroup">区域化属性类型</param>
        /// <param name="localeKey">属性键</param>
        /// <returns>实体区域化属性</returns>
        string GetLocalizedValue(int languageId, int entityId, string localeKeyGroup, string localeKey);

        /// <summary>
        /// 添加实体区域化属性
        /// </summary>
        /// <param name="localizedProperty">实体区域化属性</param>
        void InsertLocalizedProperty(LocalizedProperty localizedProperty);

        /// <summary>
        /// 保存实体区域化属性值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">泛型实例</param>
        /// <param name="keySelector">属性类型选择</param>
        /// <param name="localeValue">属性值</param>
        /// <param name="languageId">语言标识符</param>
        void SaveLocalizedValue<T>(T entity,
            Expression<Func<T, string>> keySelector,
            string localeValue,
            int languageId) where T : BaseEntity, ILocalizedEntity;

        /// <summary>
        /// 保存实体区域化属性值
        /// </summary>
        /// <typeparam name="T">实体泛型</typeparam>
        /// <typeparam name="TPropType">属性值泛型</typeparam>
        /// <param name="entity">泛型实例</param>
        /// <param name="keySelector">属性类型选择</param>
        /// <param name="localeValue">属性值</param>
        /// <param name="languageId">语言标识符</param>
        void SaveLocalizedValue<T, TPropType>(T entity,
            Expression<Func<T, TPropType>> keySelector,
            TPropType localeValue,
            int languageId) where T : BaseEntity, ILocalizedEntity;

        /// <summary>
        /// 更新实体区域化属性
        /// </summary>
        /// <param name="localizedProperty">实体区域化属性</param>
        void UpdateLocalizedProperty(LocalizedProperty localizedProperty);

        /// <summary>
        /// 删除实体区域化属性
        /// </summary>
        /// <param name="localizedProperty">实体区域化属性</param>
        void DeleteLocalizedProperty(LocalizedProperty localizedProperty);

    }
}
