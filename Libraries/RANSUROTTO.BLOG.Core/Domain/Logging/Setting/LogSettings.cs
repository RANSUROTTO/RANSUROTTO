using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Logging.Setting
{

    /// <summary>
    /// 日志设置
    /// </summary>
    public class LogSettings : ISettings
    {

        /// <summary>
        /// 获取或设置在记录错误/消息时忽略的忽略词
        /// </summary>
        public List<string> IgnoreLogWordlist { get; set; }

    }
}
