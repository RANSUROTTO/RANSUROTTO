
using System;

namespace RANSUROTTO.BLOG.Core.Extensions
{

    /// <summary>
    /// 一个常用的扩展类
    /// </summary>
    public static class CommonExtensions
    {

        /// <summary>
        /// 检查可空类型对象值(或默认值)是否为其结果类型的默认值
        /// </summary>
        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }

        public static TResult Return<TInput, TResult>(this TInput o, Func<TInput, TResult> evaluator, TResult failureValue)
            where TInput : class
        {
            return o == null ? failureValue : evaluator(o);
        }

    }

}
