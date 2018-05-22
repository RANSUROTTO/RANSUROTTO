using System.Collections.Generic;
using RANSUROTTO.BLOG.Core.Domain.Tasks;

namespace RANSUROTTO.BLOG.Services.Tasks
{

    /// <summary>
    /// 计划任务业务层接口
    /// </summary>
    public interface IScheduleTaskService
    {

        /// <summary>
        /// 通过标识符获取计划任务
        /// </summary>
        /// <param name="taskId">计划任务标识符</param>
        /// <returns>计划任务</returns>
        ScheduleTask GetTaskById(int taskId);

        /// <summary>
        /// 通过类型获取计划任务
        /// </summary>
        /// <param name="type">计划任务类型</param>
        /// <returns>计划任务</returns>
        ScheduleTask GetTaskByType(string type);

        /// <summary>
        /// 获取所有计划任务
        /// </summary>
        /// <param name="showHidden">显示隐藏的记录</param>
        /// <returns>计划任务列表</returns>
        IList<ScheduleTask> GetAllTasks(bool showHidden = false);

        /// <summary>
        /// 添加任务计划
        /// </summary>
        /// <param name="task">任务计划</param>
        void InsertTask(ScheduleTask task);

        /// <summary>
        /// 更新任务计划
        /// </summary>
        /// <param name="task">任务计划</param>
        void UpdateTask(ScheduleTask task);

        /// <summary>
        /// 删除任务计划
        /// </summary>
        /// <param name="task">任务计划</param>
        void DeleteTask(ScheduleTask task);

    }
}
