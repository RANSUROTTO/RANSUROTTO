using RANSUROTTO.BLOG.Core.Data;

namespace RANSUROTTO.BLOG.Core.Domain.Configuration
{

    public class Setting : BaseEntity
    {

        public Setting() { }

        public Setting(string name, string value)
        {
            Name = name;
            Value = value;
        }

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ��ȡ������ֵ
        /// </summary>
        public string Value { get; set; }

    }

}
