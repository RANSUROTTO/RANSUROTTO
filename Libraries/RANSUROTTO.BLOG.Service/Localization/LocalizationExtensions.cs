using System;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Domain.Localization;
using RANSUROTTO.BLOG.Core.Domain.Security;
using RANSUROTTO.BLOG.Core.Helper;

namespace RANSUROTTO.BLOG.Service.Localization
{
    public static class LocalizationExtensions
    {

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
        public static string GetLocalizedEnum<T>(this T enumValue, ILocalizationService localizationService, long languageId)
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
