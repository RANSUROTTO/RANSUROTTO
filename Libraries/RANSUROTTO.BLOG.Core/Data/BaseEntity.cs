using System;

namespace RANSUROTTO.BLOG.Core.Data
{
    public abstract class BaseEntity
    {

        #region Constructor

        protected BaseEntity()
        {
            this.Guid = new Guid();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 获取或设置实体主键标识符
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 获取或设置实体全球唯一标识符
        /// </summary>
        public virtual Guid Guid { get; set; }

        /// <summary>
        /// 获取或设置实体创建时的UTC时间
        /// </summary>
        public virtual DateTime CreatedOnUtc { get; set; }

        /// <summary>
        /// 获取或设置实体并发标识
        /// </summary>
        public virtual byte[] TimeStamp { get; set; }

        /// <summary>
        /// 获取或设置实体是否已被软删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        #endregion

        #region Methods

        public static bool operator ==(BaseEntity x, BaseEntity y) => Equals(x, y);

        public static bool operator !=(BaseEntity x, BaseEntity y) => !(x == y);

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (object.Equals(this.Id, other.Id))
            {
                return this.GetType() == other.GetType();
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        #endregion

    }

}
