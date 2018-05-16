using RANSUROTTO.BLOG.Services.Tasks;

namespace RANSUROTTO.BLOG.Services.Logging
{
    /// <summary>
    /// 清除日志计划任务实现
    /// </summary>
    public class ClearLogTask : ITask
    {

        private readonly ILogger _logger;

        public ClearLogTask(ILogger logger)
        {
            _logger = logger;
        }

        public virtual void Execute()
        {
            _logger.ClearLog();
        }

    }
}
