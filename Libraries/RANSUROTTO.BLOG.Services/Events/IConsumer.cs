namespace RANSUROTTO.BLOG.Services.Events
{

    /// <summary>
    /// 触发对象接口
    /// </summary>
    /// <typeparam name="T">类型</typeparam>
    public interface IConsumer<T>
    {

        /// <summary>
        /// 处理事件
        /// </summary>
        /// <param name="eventMessage">事件</param>
        void HandleEvent(T eventMessage);

    }
}
