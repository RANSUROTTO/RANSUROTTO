using System;
using System.Linq.Expressions;
using System.Reflection;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Data;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Domain.Security;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Infrastructure;

namespace RANSUROTTO.BLOG.Services.Localization
{
    public static class LocalizationExtensions
    {

        /// <summary>
        /// 获取实体区域化属性值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">对应实体</param>
        /// <param name="keySelector">属性类型选择</param>
        /// <returns>实体区域化属性值</returns>
        public static string GetLocalized<T>(this T entity,
            Expression<Func<T, string>> keySelector)
            where T : BaseEntity, ILocalizedEntity
        {
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            return GetLocalized(entity, keySelector, workContext.WorkingLanguage.Id);
        }

        /// <summary>
        /// 获取实体区域化属性值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="entity">对应实体</param>
        /// <param name="keySelector">属性类型选择</param>
        /// <param name="languageId">语言标识符</param>
        /// <param name="returnDefaultValue">指示是否返回默认值（如果没有找到属性值）</param>
        /// <param name="ensureTwoPublishedLanguages">指示是否确保至少有两种已发布语言,否则只加载默认值</param>
        /// <returns>实体区域化属性值</returns>
        public static string GetLocalized<T>(this T entity,
            Expression<Func<T, string>> keySelector, int languageId,
            bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where T : BaseEntity, ILocalizedEntity
        {
            return GetLocalized<T, string>(entity, keySelector, languageId, returnDefaultValue, ensureTwoPublishedLanguages);
        }

        /// <summary>
        /// 获取实体区域化属性值
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <typeparam name="TPropType">值泛型</typeparam>
        /// <param name="entity">对应实体</param>
        /// <param name="keySelector">属性类型选择</param>
        /// <param name="languageId">语言标识符</param>
        /// <param name="returnDefaultValue">指示是否返回默认值（如果没有找到属性值）</param>
        /// <param name="ensureTwoPublishedLanguages">指示是否确保至少有两种已发布语言,否则只加载默认值</param>
        /// <returns>实体区域化属性值</returns>
        public static TPropType GetLocalized<T, TPropType>(this T entity,
            Expression<Func<T, TPropType>> keySelector, int languageId,
            bool returnDefaultValue = true, bool ensureTwoPublishedLanguages = true)
            where T : BaseEntity, ILocalizedEntity
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

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

            TPropType result = default(TPropType);
            string resultStr = string.Empty;

            //加载区域化值
            string localeKeyGroup = typeof(T).Name;
            string localeKey = propInfo.Name;

            if (languageId > 0)
            {
                bool loadLocalizedValue = true;
                if (ensureTwoPublishedLanguages)
                {
                    var lService = EngineContext.Current.Resolve<ILanguageService>();
                    var totalPublishedLanguages = lService.GetAllLanguages().Count;
                    loadLocalizedValue = totalPublishedLanguages >= 2;
                }

                if (loadLocalizedValue)
                {
                    var leService = EngineContext.Current.Resolve<ILocalizedEntityService>();
                    resultStr = leService.GetLocalizedValue(languageId, entity.Id, localeKeyGroup, localeKey);
                    if (!String.IsNullOrEmpty(resultStr))
                        result = CommonHelper.To<TPropType>(resultStr);
                }
            }

            if (string.IsNullOrEmpty(resultStr) && returnDefaultValue)
            {
                var localizer = keySelector.Compile();
                result = localizer(entity);
            }

            return result;
        }

        /// <summary>
        /// 获取枚举区域化显示值
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <param name="localizationService">区域化服务实例</param>
        /// <param name="workContext">工作上下文</param>
        /// <returns>区域化显示值</returns>
        public static string GetLocalizedEnum<T>(this T enumValue, ILocalizationService localizationService, IWorkContext workContext)
            where T : struct
        {
            if (workContext == null)
                throw new ArgumentNullException(nameof(workContext));

            return GetLocalizedEnum(enumValue, localizationService, workContext.WorkingLanguage.Id);
        }

        /// <summary>
        /// 获取枚举区域化显示值
        /// </summary>
        /// <typeparam name="T">枚举</typeparam>
        /// <param name="enumValue">枚举值</param>
        /// <param name="localizationService">区域化服务实例</param>
        /// <param name="languageId">语言标识符</param>
        /// <returns>区域化显示值</returns>
        public static string GetLocalizedEnum<T>(this T enumValue, ILocalizationService localizationService, int languageId)
            where T : struct
        {
            if (localizationService == null)
                throw new ArgumentNullException(nameof(localizationService));

            if (!typeof(T).IsEnum) throw new ArgumentException("泛型T必须是枚举值类型!");

            string resourceName = string.Format("Enums.{0}.{1}",
                typeof(T).FullName,
                enumValue.ToString());

            string result = localizationService.GetResource(resourceName, languageId, false, "", true);

            //设置默认值
            if (string.IsNullOrEmpty(result))
                result = CommonHelper.ConvertEnum(enumValue.ToString());

            return result;
        }

        /// <summary>
        /// 获取权限项显示名称区域化资源
        /// </summary>
        /// <param name="permissionRecord">权限项</param>
        /// <param name="localizationService">区域化服务实例</param>
        /// <param name="workContext">工作区上下文</param>
        /// <returns></returns>
        public static string GetLocalizedPermissionName(this PermissionRecord permissionRecord,
            ILocalizationService localizationService, IWorkContext workContext)
        {
            if (workContext == null)
                throw new ArgumentNullException(nameof(workContext));

            return GetLocalizedPermissionName(permissionRecord, localizationService, workContext.WorkingLanguage.Id);
        }

        /// <summary>
        /// 获取权限项显示名称区域化资源
        /// </summary>
        /// <param name="permissionRecord">权限项</param>
        /// <param name="localizationService">区域化服务实例</param>
        /// <param name="languageId">语言标识符</param>
        /// <returns>权限项区域化显示名称</returns>
        public static string GetLocalizedPermissionName(this PermissionRecord permissionRecord,
            ILocalizationService localizationService, int languageId)
        {
            if (permissionRecord == null)
                throw new ArgumentNullException(nameof(permissionRecord));

            if (localizationService == null)
                throw new ArgumentNullException(nameof(localizationService));

            string resourceName = $"Permission.{permissionRecord.SystemName}";
            string result = localizationService.GetResource(resourceName, languageId, false, "", true);

            if (string.IsNullOrEmpty(result))
                result = permissionRecord.Name;

            return result;
        }

        /// <summary>
        /// 保存权限项显示名称至区域化资源
        /// </summary>
        /// <param name="permissionRecord">权限项</param>
        /// <param name="localizationService">区域化服务实例</param>
        /// <param name="languageService">语言服务实例</param>
        public static void SaveLocalizedPermissionName(this PermissionRecord permissionRecord,
            ILocalizationService localizationService, ILanguageService languageService)
        {
            if (permissionRecord == null)
                throw new ArgumentNullException(nameof(permissionRecord));
            if (localizationService == null)
                throw new ArgumentNullException(nameof(localizationService));
            if (languageService == null)
                throw new ArgumentNullException(nameof(languageService));

            string resourceName = $"Permission.{permissionRecord.SystemName}";
            string resourceValue = permissionRecord.Name;

            foreach (var lang in languageService.GetAllLanguages(true))
            {
                var lsr = localizationService.GetLocaleStringResourceByName(resourceName, lang.Id, false);
                if (lsr == null)
                {
                    lsr = new LocaleStringResource
                    {
                        LanguageId = lang.Id,
                        ResourceName = resourceName,
                        ResourceValue = resourceValue
                    };
                    localizationService.InsertLocaleStringResource(lsr);
                }
                else
                {
                    lsr.ResourceValue = resourceValue;
                    localizationService.UpdateLocaleStringResource(lsr);
                }
            }
        }

        /// <summary>
        /// 删除权限项显示名称区域化资源
        /// </summary>
        /// <param name="permissionRecord">权限项</param>
        /// <param name="localizationService">区域化服务实例</param>
        /// <param name="languageService">语言服务实例</param>
        public static void DeleteLocalizedPermissionName(this PermissionRecord permissionRecord,
            ILocalizationService localizationService, ILanguageService languageService)
        {
            if (permissionRecord == null)
                throw new ArgumentNullException(nameof(permissionRecord));
            if (localizationService == null)
                throw new ArgumentNullException(nameof(localizationService));
            if (languageService == null)
                throw new ArgumentNullException(nameof(languageService));

            string resourceName = $"Permission.{permissionRecord.SystemName}";
            foreach (var lang in languageService.GetAllLanguages(true))
            {
                var lsr = localizationService.GetLocaleStringResourceByName(resourceName, lang.Id, false);
                if (lsr != null)
                    localizationService.DeleteLocaleStringResource(lsr);
            }
        }

    }
}
