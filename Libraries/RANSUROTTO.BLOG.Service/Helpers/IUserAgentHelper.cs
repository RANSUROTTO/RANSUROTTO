namespace RANSUROTTO.BLOG.Services.Helpers
{
    /// <summary>
    /// 用户代理帮助类接口
    /// </summary>
    public interface IUserAgentHelper
    {

        /// <summary>
        /// 获取一个值，该值指示请求是否为搜索引擎（网络爬虫）
        /// </summary>
        /// <returns>结果</returns>
        bool IsSearchEngine();

    }
}
