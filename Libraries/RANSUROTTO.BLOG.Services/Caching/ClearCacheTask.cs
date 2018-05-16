using RANSUROTTO.BLOG.Core.Caching;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Services.Tasks;

namespace RANSUROTTO.BLOG.Services.Caching
{
    /// <summary>
    /// 清除缓存的计划任务实现
    /// </summary>
    public class ClearCacheTask : ITask
    {
        public virtual void Execute()
        {
            var cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("ransurotto_cache_static");
            cacheManager.Clear();
        }
    }
}
