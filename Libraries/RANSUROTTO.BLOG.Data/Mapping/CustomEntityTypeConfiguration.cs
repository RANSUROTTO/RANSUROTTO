using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Data.Mapping
{

    public abstract class CustomEntityTypeConfiguration<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {

        protected CustomEntityTypeConfiguration()
        {
            this.Initialize();
            this.HasKey(p => p.Id);
            this.Property(p => p.TimeStamp)
                .IsConcurrencyToken(true)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
        }

        /// <summary>
        /// 可以重写这一个方法
        /// 实现通过构造函数执行该自定义初始化代码
        /// </summary>
        protected virtual void Initialize()
        { }

    }

}
