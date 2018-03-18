using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace RANSUROTTO.BLOG.Core.Caching
{
    public static class CacheManagerExtensions
    {

        /// <summary>
        /// 获取缓存项，如果它不在缓存列表中，则执行指定委托函数
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cacheManager">缓存管理实例</param>
        /// <param name="key">缓存项键</param>
        /// <param name="acquire">如果为找到缓存项，则执行的委托函数</param>
        /// <returns>缓存项</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
        {
            return Get(cacheManager, key, 60, acquire);
        }

        /// <summary>
        /// 获取缓存项，如果它不在缓存列表中，则执行指定委托函数
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="cacheManager">缓存管理实例</param>
        /// <param name="key">缓存项键</param>
        /// <param name="cacheTime">缓存时间/分钟</param>
        /// <param name="acquire">如果为找到缓存项，则执行的委托函数</param>
        /// <returns>缓存项</returns>
        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
        {
            if (cacheManager.IsSet(key))
            {
                return cacheManager.Get<T>(key);
            }

            var result = acquire();
            if (cacheTime > 0)
                cacheManager.Set(key, result, cacheTime);
            return result;
        }

        /// <summary>
        /// 按指定模式(正则表达式)匹配删除对应键的缓存项
        /// </summary>
        /// <param name="cacheManager">缓存管理实例</param>
        /// <param name="pattern">匹配模式</param>
        /// <param name="keys">缓存键列表</param>
        public static void RemoveByPattern(this ICacheManager cacheManager, string pattern, IEnumerable<string> keys)
        {
            var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
            foreach (var key in keys.Where(p => regex.IsMatch(p.ToString())).ToList())
                cacheManager.Remove(key);
        }

    }
}
