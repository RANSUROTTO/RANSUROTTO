using System;
using Autofac;
using RANSUROTTO.BLOG.Core.Caching.RedisCaching;
using RANSUROTTO.BLOG.Core.Configuration;
using RANSUROTTO.BLOG.Core.Domain.Tasks;
using RANSUROTTO.BLOG.Core.Infrastructure;
using RANSUROTTO.BLOG.Services.Infrastructure;
using RANSUROTTO.BLOG.Services.Logging;

namespace RANSUROTTO.BLOG.Services.Tasks
{
    public partial class Task
    {

        #region Constructor

        private Task()
        {
            this.Enabled = true;
        }

        public Task(ScheduleTask task)
        {
            this.Type = task.Type;
            this.Enabled = task.Enabled;
            this.StopOnError = task.StopOnError;
            this.Name = task.Name;
            this.LastSuccessUtc = task.LastSuccessUtc;
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置最后一次开始执行Utc时间
        /// </summary>
        public DateTime? LastStartUtc { get; private set; }

        /// <summary>
        /// 获取或设置最后一次执行完毕Utc时间(无论成功或失败)
        /// </summary>
        public DateTime? LastEndUtc { get; private set; }

        /// <summary>
        /// 获取或设置最后一次执行成功Utc时间
        /// </summary>
        public DateTime? LastSuccessUtc { get; private set; }

        /// <summary>
        /// 获取或设置任务的类型
        /// </summary>
        public string Type { get; private set; }

        /// <summary>
        /// 获取或设置一个值，标识遇到错误时是否停止
        /// </summary>
        public bool StopOnError { get; private set; }

        /// <summary>
        /// 获取或设置任务名称
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// 获取或设置一个值，标识是否已启用
        /// </summary>
        public bool Enabled { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// 执行任务
        /// </summary>
        /// <param name="throwException">标识是否在遇到错误时抛出异常</param>
        /// <param name="dispose">标识所有实例是否在任务运行后销毁</param>
        /// <param name="ensureRunOnOneWebFarmInstance">标识是否应确保该任务一次在一个节点上运行</param>
        public void Execute(bool throwException = false, bool dispose = true, bool ensureRunOnOneWebFarmInstance = true)
        {
            var scope = EngineContext.Current.ContainerManager.Scope();
            var scheduleTaskService = EngineContext.Current.ContainerManager.Resolve<IScheduleTaskService>("", scope);
            var scheduleTask = scheduleTaskService.GetTaskByType(this.Type);

            try
            {
                //初始化标识任务是否已执行为否
                var taskExecuted = false;

                //确保任务一次在一个节点上运行
                if (ensureRunOnOneWebFarmInstance)
                {
                    var webConfig = EngineContext.Current.ContainerManager.Resolve<WebConfig>("", scope);
                    if (webConfig.MultipleInstancesEnabled)
                    {
                        var machineNameProvider = EngineContext.Current.ContainerManager.Resolve<IMachineNameProvider>("", scope);
                        var machineName = machineNameProvider.GetMachineName();
                        if (String.IsNullOrEmpty(machineName))
                        {
                            throw new Exception("无法检测到机器名。不能在Web站点中运行。");
                        }

                        if (scheduleTask != null)
                        {
                            if (webConfig.RedisCachingEnable)
                            {
                                var expirationInSeconds = scheduleTask.Seconds <= 300 ? scheduleTask.Seconds - 1 : 300;

                                var executeTaskAction = new Action(() =>
                                {
                                    taskExecuted = true;
                                    var task = this.CreateTask(scope);
                                    if (task != null)
                                    {
                                        scheduleTask.LastStartUtc = DateTime.UtcNow;
                                        scheduleTaskService.UpdateTask(scheduleTask);
                                        task.Execute();
                                        this.LastEndUtc = this.LastSuccessUtc = DateTime.UtcNow;
                                    }
                                });

                                var redisWrapper = EngineContext.Current.ContainerManager.Resolve<IRedisConnectionWrapper>(scope: scope);
                                if (!redisWrapper.PerformActionWithLock(scheduleTask.Type, TimeSpan.FromSeconds(expirationInSeconds), executeTaskAction))
                                    return;
                            }
                            else
                            {
                                //lease can't be acquired only if for a different machine and it has not expired
                                if (scheduleTask.LeasedUntilUtc.HasValue &&
                                    scheduleTask.LeasedUntilUtc.Value >= DateTime.UtcNow &&
                                    scheduleTask.LeasedByMachineName != machineName)
                                    return;

                                //lease the task. so it's run on one farm node at a time
                                scheduleTask.LeasedByMachineName = machineName;
                                scheduleTask.LeasedUntilUtc = DateTime.UtcNow.AddMinutes(30);
                                scheduleTaskService.UpdateTask(scheduleTask);
                            }
                        }
                    }
                }

                if (!taskExecuted)
                {
                    //初始化并且执行
                    var task = this.CreateTask(scope);
                    if (task != null)
                    {
                        this.LastStartUtc = DateTime.UtcNow;
                        if (scheduleTask != null)
                        {
                            //更新运行后的时间属性
                            scheduleTask.LastStartUtc = this.LastStartUtc;
                            scheduleTaskService.UpdateTask(scheduleTask);
                        }
                        task.Execute();
                        this.LastEndUtc = this.LastSuccessUtc = DateTime.UtcNow;
                    }
                }

            }
            catch (Exception exc)
            {
                this.Enabled = !this.StopOnError;
                this.LastEndUtc = DateTime.UtcNow;

                //记录日志错误
                var logger = EngineContext.Current.ContainerManager.Resolve<ILogger>("", scope);
                logger.Error(string.Format("运行计划任务 '{0}' 发生错误. {1}", this.Name, exc.Message), exc);
                if (throwException)
                    throw;
            }

            if (scheduleTask != null)
            {
                //更新任务时间属性
                scheduleTask.LastEndUtc = this.LastEndUtc;
                scheduleTask.LastSuccessUtc = this.LastSuccessUtc;
                scheduleTaskService.UpdateTask(scheduleTask);
            }

            //销毁所有资源
            if (dispose)
            {
                scope.Dispose();
            }

        }

        #endregion

        #region Utilities

        /// <summary>
        /// 创建任务
        /// </summary>
        /// <param name="scope">生命周期</param>
        /// <returns>任务实例对象</returns>
        private ITask CreateTask(ILifetimeScope scope)
        {
            ITask task = null;
            if (this.Enabled)
            {
                var type2 = System.Type.GetType(this.Type);
                if (type2 != null)
                {
                    if (!EngineContext.Current.ContainerManager.TryResolve(type2, scope, out var instance))
                    {
                        instance = EngineContext.Current.ContainerManager.ResolveUnregistered(type2, scope);
                    }
                    task = instance as ITask;
                }
            }
            return task;
        }

        #endregion

    }
}
