using RANSUROTTO.BLOG.Core.Domain.Common;

namespace RANSUROTTO.BLOG.Data.Mapping.Common
{
    public class GenericAttributeMap : CustomEntityTypeConfiguration<GenericAttribute>
    {
        public GenericAttributeMap()
        {
            this.ToTable("GenericAttribute");

            this.Property(p => p.KeyGroup).IsRequired().HasMaxLength(400);
            this.Property(p => p.Key).IsRequired().HasMaxLength(400);
            this.Property(p => p.Value).IsRequired();
        }
    }
}
