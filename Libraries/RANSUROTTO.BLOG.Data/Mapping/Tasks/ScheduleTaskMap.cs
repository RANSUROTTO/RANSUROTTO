using RANSUROTTO.BLOG.Core.Domain.Tasks;

namespace RANSUROTTO.BLOG.Data.Mapping.Tasks
{
    public class ScheduleTaskMap : CustomEntityTypeConfiguration<ScheduleTask>
    {
        public ScheduleTaskMap()
        {
            this.ToTable("ScheduleTask");

            this.Property(t => t.Name).IsRequired();
            this.Property(t => t.Type).IsRequired();
        }
    }

}
