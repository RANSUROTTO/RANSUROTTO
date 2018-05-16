using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Infrastructure;

namespace RANSUROTTO.BLOG.Services.Events
{
    /// <summary>
    /// 事件订阅业务
    /// </summary>
    public class SubscriptionService : ISubscriptionService
    {

        /// <summary>
        /// 获取订阅列表
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>已注册订阅列表</returns>
        public IList<IConsumer<T>> GetSubscriptions<T>()
        {
            return EngineContext.Current.ResolveAll<IConsumer<T>>();
        }

    }
}
