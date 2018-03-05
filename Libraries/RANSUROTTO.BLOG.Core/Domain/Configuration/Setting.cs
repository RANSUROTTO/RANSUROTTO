namespace RANSUROTTO.BLOG.Core.Domain.Configuration
{

    public class Setting
    {

        public Setting() { }

        public Setting(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// 获取或设置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置值
        /// </summary>
        public string Value { get; set; }

    }

}
