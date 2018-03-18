namespace RANSUROTTO.BLOG.Service.Events
{
    /// <summary>
    /// 事件发布接口
    /// </summary>
    public interface IEventPublisher
    {

        /// <summary>
        /// 发布事件
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="eventMessage">事件消息</param>
        void Publish<T>(T eventMessage);

    }
}
