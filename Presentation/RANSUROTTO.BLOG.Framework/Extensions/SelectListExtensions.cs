using System;
using System.Linq;
using System.Web.Mvc;
using RANSUROTTO.BLOG.Core.Data;
using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Helper;
using RANSUROTTO.BLOG.Core.Context;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Services.Localization;

namespace RANSUROTTO.BLOG.Framework.Extensions
{
    public static class SelectListExtensions
    {

        /// <summary>
        /// 将枚举处理为选择列表
        /// </summary>
        /// <typeparam name="TEnum">枚举类型</typeparam>
        /// <param name="enumObj">当前枚举值</param>
        /// <param name="markCurrentAsSelected">标识是否将当前枚举值设置为选中值</param>
        /// <param name="valuesToExclude">排除的值</param>
        /// <param name="useLocalization">是否应用区域化处理</param>
        /// <returns>枚举选择列表</returns>
        public static SelectList ToSelectList<TEnum>(this TEnum enumObj,
            bool markCurrentAsSelected = true, int[] valuesToExclude = null, bool useLocalization = true) where TEnum : struct
        {
            if (!typeof(TEnum).IsEnum) throw new ArgumentException("TEnum泛型必须为枚举类型.", nameof(enumObj));

            var localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            var workContext = EngineContext.Current.Resolve<IWorkContext>();

            var values = from TEnum enumValue in Enum.GetValues(typeof(TEnum))
                         where valuesToExclude == null || !valuesToExclude.Contains(Convert.ToInt32(enumValue))
                         select new { ID = Convert.ToInt32(enumValue), Name = useLocalization ? enumValue.GetLocalizedEnum(localizationService, workContext) : CommonHelper.ConvertEnum(enumValue.ToString()) };
            object selectedValue = null;
            if (markCurrentAsSelected)
                selectedValue = Convert.ToInt32(enumObj);
            return new SelectList(values, "ID", "Name", selectedValue);
        }

        /// <summary>
        /// 实体集合处理为选择列表
        /// </summary>
        /// <typeparam name="T">实体集合</typeparam>
        /// <param name="objList">实体集合实例</param>
        /// <param name="selector">显示值选择器</param>
        /// <returns>实体选择列表</returns>
        public static SelectList ToSelectList<T>(this T objList, Func<BaseEntity, string> selector) where T : IEnumerable<BaseEntity>
        {
            return new SelectList(objList.Select(p => new { ID = p.Id, Name = selector(p) }), "ID", "Name");
        }

    }
}
