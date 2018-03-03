using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Localization
{

    public class LocaleStringResource : BaseEntity
    {

        /// <summary>
        /// 获取或设置资源名称
        /// </summary>
        public string ResourceName { get; set; }

        /// <summary>
        /// 获取或设置资源值
        /// </summary>
        public string ResourceValue { get; set; }

        public long LanguageId { get; set; }
        /// <summary>
        /// 获取或设置语言身份
        /// </summary>
        public Language Language { get; set; }

    }

}
