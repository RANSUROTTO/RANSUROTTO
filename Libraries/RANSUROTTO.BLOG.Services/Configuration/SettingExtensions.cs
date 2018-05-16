using System;
using System.Linq.Expressions;
using System.Reflection;
using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Services.Configuration
{
    public static class SettingExtensions
    {

        /// <summary>
        /// 获取设定选择器选中的设定项的键
        /// </summary>
        /// <typeparam name="T">设定类型</typeparam>
        /// <typeparam name="TPropType">设定项类型</typeparam>
        /// <param name="entity">设定实例</param>
        /// <param name="keySelector">设定项选择</param>
        /// <returns>键</returns>
        public static string GetSettingKey<T, TPropType>(this T entity,
            Expression<Func<T, TPropType>> keySelector)
            where T : ISettings, new()
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
            {
                throw new ArgumentException(string.Format(
                    "表达式 '{0}' 指向一个方法, 不是一个属性。",
                    keySelector));
            }

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
            {
                throw new ArgumentException(string.Format(
                    "表达式 '{0}' 指向一个字段, 不是一个属性。",
                    keySelector));
            }

            var key = typeof(T).Name + "." + propInfo.Name;
            return key;
        }

    }
}
