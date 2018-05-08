using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;

namespace RANSUROTTO.BLOG.Services.Tasks
{
    public class TaskThread : IDisposable
    {

        #region Fields

        private Timer _timer;
        private bool _disposed;
        private readonly Dictionary<string, Task> _tasks;

        #endregion

        #region Constructor

        internal TaskThread()
        {
            this._tasks = new Dictionary<string, Task>();
            this.Seconds = 10 * 60;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置运行周期间隔(秒)
        /// </summary>
        public int Seconds { get; set; }

        /// <summary>
        /// 获取或设置线程已启动Utc时间
        /// </summary>
        public DateTime StartedUtc { get; private set; }

        /// <summary>
        /// 获取或设置线程是否在运行
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 获取或设置该线程是否仅应运行一次(每次应用启动重置)
        /// </summary>
        public bool RunOnlyOnce { get; set; }

        /// <summary>
        /// 获取任务列表
        /// </summary>
        public IList<Task> Tasks
        {
            get
            {
                var list = new List<Task>();
                foreach (var task in this._tasks.Values)
                {
                    list.Add(task);
                }
                return new ReadOnlyCollection<Task>(list);
            }
        }

        /// <summary>
        /// 获取或设置运行周期间隔(毫秒)
        /// </summary>
        public int Interval
        {
            get
            {
                //如果Seconds超过2147483秒将会抛出异常
                int interval = this.Seconds * 1000;
                if (interval <= 0)
                    interval = int.MaxValue;
                return interval;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 初始化定时器
        /// </summary>
        public void InitTimer()
        {
            if (this._timer == null)
            {
                this._timer = new Timer(this.TimerHandler, null, this.Interval, this.Interval);
            }
        }

        /// <summary>
        /// 执行任务
        /// </summary>
        private void Run()
        {
            if (Seconds <= 0)
                return;

            this.StartedUtc = DateTime.UtcNow;
            this.IsRunning = true;
            foreach (Task task in this._tasks.Values)
            {
                task.Execute();
            }
            this.IsRunning = false;
        }

        /// <summary>
        /// 定时器处理程序
        /// </summary>
        /// <param name="state"></param>
        private void TimerHandler(object state)
        {
            this._timer.Change(-1, -1);
            this.Run();
            if (this.RunOnlyOnce)
            {
                this.Dispose();
            }
            else
            {
                this._timer.Change(this.Interval, this.Interval);
            }
        }

        /// <summary>
        /// 添加任务至任务队列
        /// </summary>
        /// <param name="task">任务</param>
        public void AddTask(Task task)
        {
            if (!this._tasks.ContainsKey(task.Name))
            {
                this._tasks.Add(task.Name, task);
            }
        }

        /// <summary>
        /// 对象销毁
        /// </summary>
        public void Dispose()
        {
            if (this._timer != null && !this._disposed)
            {
                lock (this)
                {
                    this._timer.Dispose();
                    this._timer = null;
                    this._disposed = true;
                }
            }
        }

        #endregion

    }
}
