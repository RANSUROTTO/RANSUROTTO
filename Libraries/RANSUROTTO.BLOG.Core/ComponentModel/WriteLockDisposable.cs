using System;
using System.Threading;

namespace RANSUROTTO.BLOG.Core.ComponentModel
{

    /// <summary>
    /// 提供对资源锁定访问提供一种便利的方法
    /// </summary>
    public class WriteLockDisposable : IDisposable
    {

        private readonly ReaderWriterLockSlim _rwLock;

        /// <summary>
        /// 初始化一个新实例 <see cref="WriteLockDisposable"/>.
        /// </summary>
        /// <param name="rwLock">rw锁</param>
        public WriteLockDisposable(ReaderWriterLockSlim rwLock)
        {
            _rwLock = rwLock;
            _rwLock.EnterWriteLock();
        }

        void IDisposable.Dispose()
        {
            _rwLock.ExitWriteLock();
        }

    }
}
