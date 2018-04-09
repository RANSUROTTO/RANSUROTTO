using System;
using RANSUROTTO.BLOG.Core.Context;
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

    }
}
