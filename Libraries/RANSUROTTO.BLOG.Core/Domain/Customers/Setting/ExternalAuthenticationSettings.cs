using RANSUROTTO.BLOG.Core.Configuration;

namespace RANSUROTTO.BLOG.Core.Domain.Customers.Setting
{
    public class ExternalAuthenticationSettings : ISettings
    {

        public bool AutoRegisterEnabled { get; set; }

    }
}
