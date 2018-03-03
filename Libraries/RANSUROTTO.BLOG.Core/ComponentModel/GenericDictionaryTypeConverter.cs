using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace RANSUROTTO.BLOG.Core.ComponentModel
{
    public class GenericDictionaryTypeConverter<TK, TV> : TypeConverter
    {

        protected readonly TypeConverter typeConverterKey;
        protected readonly TypeConverter typeConverterValue;

        #region Constructor

        /// <summary>
        /// Ctor
        /// </summary>
        public GenericDictionaryTypeConverter()
        {
            typeConverterKey = TypeDescriptor.GetConverter(typeof(TK));
            if (typeConverterKey == null)
                throw new InvalidOperationException(("No type converter exists for type " + typeof(TK).FullName));

            typeConverterValue = TypeDescriptor.GetConverter(typeof(TV));
            if (typeConverterValue == null)
                throw new InvalidOperationException(("No type converter exists for type " + typeof(TV).FullName));
        }

        #endregion

        #region Methods

        /// <summary>
        /// 获取一个值，表示该转换器是否可以
        /// 将给定源类型中的对象转换为转换器的类型
        /// </summary>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// 将给定对象转换为转换器的类型
        /// 目前实现的例子:
        /// string:"1,2;3,4" => dictionary:{{1,2},{3,4}}
        /// </summary>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var input = value as string;
            if (input != null)
            {
                string[] items = string.IsNullOrEmpty(input)
                    ? new string[0]
                    : input.Split(';').Select(p => p.Trim()).ToArray();

                var result = new Dictionary<TK, TV>();
                Array.ForEach(items, p =>
                {
                    string[] keyValueStr = string.IsNullOrEmpty(p)
                        ? new string[0]
                        : p.Split(',').Select(x => x.Trim()).ToArray();

                    if (keyValueStr.Length == 2)
                    {
                        object dictionaryKey = (TK)typeConverterKey.ConvertFromInvariantString(keyValueStr[0]);
                        object dictionaryValue = (TV)typeConverterValue.ConvertFromInvariantString(keyValueStr[1]);
                        if (dictionaryKey != null && dictionaryValue != null)
                        {
                            if (!result.ContainsKey((TK)dictionaryKey))
                                result.Add((TK)dictionaryKey, (TV)dictionaryValue);
                        }
                    }
                });
                return result;
            }
            return base.ConvertFrom(context, culture, value);
        }

        /// <summary>
        /// 将给定对象转换为转换器的类型
        /// 目前实现的例子:
        /// dictionary:{{1,2},{3,4}} => string:"1,2;3,4" 
        /// </summary>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                StringBuilder result = new StringBuilder();
                if (value != null)
                {
                    int counter = 0;
                    var dictionary = (IDictionary<TK, TV>)value;
                    foreach (var keyValue in dictionary)
                    {
                        result.Append(string.Format("{0},{1}", Convert.ToString(keyValue.Key, CultureInfo.InvariantCulture), Convert.ToString(keyValue.Value, CultureInfo.InvariantCulture)));

                        if (counter != dictionary.Count - 1)
                            result.Append(";");

                        counter++;
                    }
                }
                return result.ToString();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion

    }

}
