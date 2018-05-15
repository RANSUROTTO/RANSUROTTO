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

        /// <summary>
        /// 获取或设置对应语言ID
        /// </summary>
        public long LanguageId { get; set; }

        #region Navigation Properties

        /// <summary>
        /// 获取或设置对应语言
        /// </summary>
        public virtual Language Language { get; set; }

        #endregion

    }

}
