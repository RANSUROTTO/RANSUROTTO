using RANSUROTTO.BLOG.Core.Domain.Logging;

namespace RANSUROTTO.BLOG.Data.Mapping.Logging
{

    public class LogMap : CustomEntityTypeConfiguration<Log>
    {
        public LogMap()
        {
            this.ToTable("Log");

            this.Property(l => l.ShortMessage).IsRequired();
            this.Property(l => l.IpAddress).HasMaxLength(200);

            this.Ignore(l => l.LogLevel);

            this.HasOptional(l => l.Customer)
                .WithMany()
                .HasForeignKey(l => l.CustomerId);
        }
    }

}
