
using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Events
{
    /// <summary>
    /// 用于更新实体的容器
    /// </summary>
    public class EntityUpdated<T> where T : BaseEntity
    {
        public EntityUpdated(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
