using System.Collections.Generic;
using System.Linq;

namespace RANSUROTTO.BLOG.Core.Domain.Customers.Service
{
    /// <summary>
    /// 更改密码结果
    /// </summary>
    public class ChangePasswordResult
    {

        public ChangePasswordResult()
        {
            this.Errors = new List<string>();
        }

        /// <summary>
        /// 是否更改成功
        /// </summary>
        public bool Success
        {
            get { return (!this.Errors.Any()); }
        }

        /// <summary>
        /// 错误列表
        /// </summary>
        public IList<string> Errors { get; set; }

        /// <summary>
        /// 添加错误
        /// </summary>
        /// <param name="error">错误描述</param>
        public void AddError(string error)
        {
            this.Errors.Add(error);
        }

    }
}
