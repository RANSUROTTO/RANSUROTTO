using RANSUROTTO.BLOG.Core.Domain.Common;

namespace RANSUROTTO.BLOG.Data.Mapping.Common
{
    public class GenericAttributeMap : CustomEntityTypeConfiguration<GenericAttribute>
    {
        public GenericAttributeMap()
        {
            this.ToTable("GenericAttribute");

            this.Property(ga => ga.KeyGroup).IsRequired().HasMaxLength(400);
            this.Property(ga => ga.Key).IsRequired().HasMaxLength(400);
            this.Property(ga => ga.Value).IsRequired();
        }
    }
}
