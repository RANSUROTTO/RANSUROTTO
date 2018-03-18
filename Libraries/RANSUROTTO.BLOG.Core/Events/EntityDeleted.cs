
using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Events
{
    /// <summary>
    /// 用于已删除实体的容器
    /// </summary>
    public class EntityDeleted<T> where T : BaseEntity
    {
        public EntityDeleted(T entity)
        {
            this.Entity = entity;
        }

        public T Entity { get; private set; }
    }
}
