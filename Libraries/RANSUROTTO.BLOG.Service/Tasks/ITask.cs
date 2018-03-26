namespace RANSUROTTO.BLOG.Service.Tasks
{

    /// <summary>
    /// 每个任务都应该实现的接口
    /// </summary>
    public interface ITask
    {

        /// <summary>
        /// 执行任务
        /// </summary>
        void Execute();

    }
}
