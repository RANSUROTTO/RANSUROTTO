using System.Collections.Generic;

namespace RANSUROTTO.BLOG.Services.Events
{

    /// <summary>
    /// 事件订阅业务层接口
    /// </summary>
    public interface ISubscriptionService
    {

        /// <summary>
        /// 获取订阅列表
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <returns>已注册订阅列表</returns>
        IList<IConsumer<T>> GetSubscriptions<T>();

    }

}
