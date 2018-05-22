using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Localization
{
    /// <summary>
    /// 区域化属性
    /// </summary>
    public class LocalizedProperty : BaseEntity
    {

        public int LanguageId { get; set; }

        /// <summary>
        /// 获取或设置对应实体的标识符
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// 获取或设置区域属性类别
        /// </summary>
        public string LocaleKeyGroup { get; set; }

        /// <summary>
        /// 获取或设置区域属性键
        /// </summary>
        public string LocaleKey { get; set; }

        /// <summary>
        /// 获取或设置区域属性值
        /// </summary>
        public string LocaleValue { get; set; }

        /// <summary>
        /// 获取或设置语言
        /// </summary>
        public virtual Language Language { get; set; }

    }
}
