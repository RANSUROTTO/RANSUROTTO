
using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Events
{
    /// <summary>
    /// 用于插入新实体的容器
    /// </summary>
    public class EntityInserted<T> where T : BaseEntity
    {
        public EntityInserted(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
